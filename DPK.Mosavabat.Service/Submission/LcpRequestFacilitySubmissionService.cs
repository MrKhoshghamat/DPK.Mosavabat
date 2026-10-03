using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DPK.Mosavabat.Common.Idempotency;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Entities.Models.RequestFacilities;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts.Submission;
using DPK.Mosavabat.Services.Extensions;
using DPK.Mosavabat.Services.Helper;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Submission;

public sealed class LcpRequestFacilitySubmissionService : ILcpRequestFacilitySubmissionService
{
    private static readonly TimeSpan PreparationLease = TimeSpan.FromSeconds(30);

    private readonly ILcpRequestFacilitySubmissionRepository _repo;
    private readonly IUserDatabase _userDatabase;
    private readonly IWorkflowEngineServices _workflowEngineServices;
    private readonly ICollateralTypeDatabase _collateralTypeDatabase;
    private readonly IExchangeCommercialDocDatabase _commercialDocDatabase;
    private readonly ISubContractTypeDatabase _subContractTypeDatabase;
    private readonly IFacilityGroupDatabase _facilityGroupDatabase;
    private readonly IRequestFacilityDatabase _requestFacilityDatabase;
    private readonly IContractGroupDatabaseService _contractGroupDatabaseService;
    private readonly ILoggerManager _logger;

    public LcpRequestFacilitySubmissionService(
        ILcpRequestFacilitySubmissionRepository repo,
        IUserDatabase userDatabase,
        IWorkflowEngineServices workflowEngineServices,
        ICollateralTypeDatabase collateralTypeDatabase,
        IExchangeCommercialDocDatabase commercialDocDatabase,
        ISubContractTypeDatabase subContractTypeDatabase,
        IFacilityGroupDatabase facilityGroupDatabase,
        IRequestFacilityDatabase requestFacilityDatabase,
        IContractGroupDatabaseService contractGroupDatabaseService,
        ILoggerManager logger)
    {
        _repo = repo;
        _userDatabase = userDatabase;
        _workflowEngineServices = workflowEngineServices;
        _collateralTypeDatabase = collateralTypeDatabase;
        _commercialDocDatabase = commercialDocDatabase;
        _subContractTypeDatabase = subContractTypeDatabase;
        _facilityGroupDatabase = facilityGroupDatabase;
        _requestFacilityDatabase = requestFacilityDatabase;
        _contractGroupDatabaseService = contractGroupDatabaseService;
        _logger = logger;
    }

    public async Task<MosavabatResponseDataModel?> SubmitAsync(
        RequestFacilityUpdateDataModel requestFacility, CancellationToken ct = default)
    {
        var userName = await _userDatabase.GetUserName(
            Convert.ToInt32(requestFacility.BranchCode), MosavabatClassName.LCP_RequestFacility);

        if (string.IsNullOrEmpty(userName))
            return new MosavabatResponseDataModel(default, default);

        var payloadHash = ComputePayloadHash(requestFacility);

        var reservation = await _repo.ReserveAsync(new ReserveSubmissionCommand(
            SourceSystem: "LCP",
            ProcessType: "LCP_ApprovalRequest",
            LcpId: requestFacility.LcpId,
            RequestNo: requestFacility.RequestNo,
            PayloadHash: payloadHash,
            PreparationLease: PreparationLease), ct);

        return reservation.Outcome switch
        {
            ReservationOutcome.PayloadMismatch =>
                throw new SubmissionPayloadMismatchException(reservation.Submission.Id),

            ReservationOutcome.Existing =>
                ResolveExisting(reservation.Submission),

            ReservationOutcome.Acquired =>
                await ProcessAcquiredAsync(reservation.Submission, requestFacility, userName, ct),

            _ => throw new InvalidOperationException()
        };
    }

    private static MosavabatResponseDataModel? ResolveExisting(LcpRequestFacilitySubmission s) => s.Status switch
    {
        SubmissionStatus.Completed => new MosavabatResponseDataModel(s.CaseId!.Value, s.RadNumber!.Value),
        SubmissionStatus.RejectedBeforeSend => null,
        SubmissionStatus.Preparing or SubmissionStatus.Sending or SubmissionStatus.BizagiCreated
            => throw new SubmissionInProgressException(s.Id),
        SubmissionStatus.Unknown
            => throw new SubmissionPendingReconciliationException(s.Id),
        _ => throw new InvalidOperationException()
    };

    private async Task<MosavabatResponseDataModel?> ProcessAcquiredAsync(
        LcpRequestFacilitySubmission submission,
        RequestFacilityUpdateDataModel requestFacility,
        string userName,
        CancellationToken ct)
    {
        requestFacility.GUID = submission.CorrelationId.ToString();
        await PrepareDataModelAsync(requestFacility);

        var standardModel = BizagiHelpers.CreateStandardModelForBizagi(requestFacility, userName, "LCP_ApprovalRequest");
        var xml = BizagiHelpers.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);

        var reacquired = await _repo.TryReacquireBeforeSendAsync(
            submission.Id, submission.RowVersion, submission.PayloadHash, PreparationLease, ct);

        if (reacquired is null)
        {
            var latest = await _repo.GetByIdAsync(submission.Id, ct);
            return ResolveExisting(latest!);
        }

        await _repo.MarkSendingAsync(submission.Id, reacquired.OwnerToken, ct);

        MosavabatResponseDataModel? response;
        WorkflowCallStatus callStatus;

        try
        {
            response = await _workflowEngineServices.CreateCasesAsync(xmlDocument);
            callStatus = response is { CaseId: > 0 }
                ? WorkflowCallStatus.Succeeded
                : WorkflowCallStatus.ExplicitFailure;
        }
        catch (Exception ex)
        {
            _logger.Error($"Transport failure calling Bizagi for submission {submission.Id}: {ex}");
            response = null;
            callStatus = WorkflowCallStatus.TransportFailure;
        }

        switch (callStatus)
        {
            case WorkflowCallStatus.Succeeded:
                await _repo.RecordBizagiSuccessAsync(
                    submission.Id, reacquired.OwnerToken, response!.CaseId, response.RadNumber, ct);
                await _requestFacilityDatabase.UpdateLcpRequestFacility(
                    response.CaseId, response.RadNumber, null, null, null, requestFacility.GUID);
                await _repo.MarkCompletedAsync(submission.Id, response.CaseId, response.RadNumber, ct);
                return response;

            case WorkflowCallStatus.ExplicitFailure:
                await _repo.MarkRejectedBeforeSendAsync(
                    submission.Id, reacquired.OwnerToken, errorCode: null,
                    errorMessage: "Bizagi returned an explicit error", ct);
                return null;

            default:
                await _repo.MarkUnknownAsync(
                    submission.Id, reacquired.OwnerToken, errorCode: null,
                    errorMessage: "No definitive response from Bizagi", ct);
                throw new SubmissionPendingReconciliationException(submission.Id);
        }
    }

    private static byte[] ComputePayloadHash(RequestFacilityUpdateDataModel dataModel)
    {
        var json = JsonConvert.SerializeObject(dataModel,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
    }

    private async Task PrepareDataModelAsync(RequestFacilityUpdateDataModel requestFacility)
    {
        requestFacility.LcpApprovelStatuse = "5";

        // استخراج شناسه نوع وثیقه
        if (requestFacility.Collaterals?.Collateral != null)
        {
            foreach (var collateralDataModel in requestFacility.Collaterals?.Collateral
                         .Where(c => c?.CollateralType != null)!)
            {
                var collateralTypeId = await _collateralTypeDatabase.GetId(
                    int.Parse(collateralDataModel?.CollateralType!));
                if (collateralDataModel != null) collateralDataModel.CollateralType = collateralTypeId;
            }

            foreach (var requestDocument in requestFacility.SubjectFacilities?.SubjectFacility.SelectMany(
                         subjectFacility => subjectFacility.RequestDocument.RequestDocument)!)
            {
                if (requestDocument.DocumentType != null)
                    requestDocument.DocumentType =
                        await _commercialDocDatabase.ExtractExchangeCommercialDocId(requestDocument.DocumentType);
            }

            // استخراج شناسه نوع قرارداد
            foreach (var requestDocument in requestFacility.SubjectFacilities.SubjectFacility)
            {
                requestDocument.FacilityGroup =
                    await _facilityGroupDatabase.ExtractFacilityGroupId(requestDocument.ContractType);

                if (requestDocument.SubContractType != null)
                {
                    requestDocument.SubContractType =
                        await _subContractTypeDatabase.ExtractSubContractTypeId(requestDocument.SubContractType);
                }

                if (requestDocument.ContractGroup == null) continue;
                requestDocument.ContractGroup = _contractGroupDatabaseService.ExtractContractGroupId(
                    new ExtractContractGroupIdVm { ContractGroupCode = requestDocument.ContractGroup }
                ).Result?.ContractGroupId;

                if (requestDocument.ContractGroup is "11" or "12")
                {
                    requestDocument.FacilityGroup = "315";
                }
            }

            // فرمت مبلغ وثیقه‌ها
            foreach (var item in requestFacility.Collaterals.Collateral.OfType<CollateralUpdateDataModel>())
            {
                if (item.Amount != null) item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            }
        }

        // فرمت مبلغ و پاکسازی توضیحات تسهیلات
        foreach (var item in requestFacility.SubjectFacilities?.SubjectFacility.OfType<SubjectFacilityUpdateDataModel>()!)
        {
            item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            item.Description = Regex.Replace(item.Description, "&amp;nbsp;", " ");
            item.SubjectContract = Regex.Replace(item.SubjectContract, "&amp;nbsp;", " ");
        }

        //// پر کردن رتبه و امتیاز اعتباری مشتری
        //if (requestFacility.MosavabatLegalRanking != null)
        //{
        //    var lcpCustomerRanksDataModel = new LcpCustomerRanksDataModel();
        //    var dataModel = BuildCustomerRankDataModel(requestFacility.MosavabatLegalRanking);
        //    lcpCustomerRanksDataModel.LcpCustomerRank.Add(dataModel);
        //    requestFacility.LcpCustomerRanks = lcpCustomerRanksDataModel;
        //}

        //// پر کردن رتبه و امتیاز ضامن‌ها
        //if (requestFacility.Guarantors != null)
        //{
        //    foreach (var guarantor in requestFacility.Guarantors.Guarantor)
        //    {
        //        var guarantorRankDataModel = new LcpGuarantorRanksDataModel();
        //        var dataModel = BuildGuarantorRankDataModel(guarantor);
        //        guarantorRankDataModel.LcpGuarantorRank.Add(dataModel);
        //        guarantor.GuarantorRanks = guarantorRankDataModel;
        //    }
        //}
    }

    //private static LcpCustomerRankDataModel BuildCustomerRankDataModel(
    //    MosavabatLegalRankingDataModel ranking)
    //{
    //    var dataModel = new LcpCustomerRankDataModel();

    //    if (ranking.LocalRanking != null)
    //    {
    //        dataModel.CreditRank = ranking.LocalRanking.Symbol?.ToString();
    //        dataModel.CreditScore = ranking.LocalRanking.Score != null
    //            ? Convert.ToDecimal(ranking.LocalRanking.Score)
    //            : null;
    //    }

    //    if (ranking.IranianRanking != null)
    //    {
    //        dataModel.IranianRank = ranking.IranianRanking.Symbol?.ToString();
    //        dataModel.IranianScore = ranking.IranianRanking.Score != null
    //            ? Convert.ToDecimal(ranking.IranianRanking.Score)
    //            : null;
    //    }

    //    if (ranking.ExternalRanking != null)
    //    {
    //        var ext = ranking.ExternalRanking;
    //        dataModel.ExternalRankAssignDate = ext.RateAssignDate != null
    //            ? Convert.ToDateTime(ext.RateAssignDate) : null;
    //        dataModel.ExternalRankExpireDate = ext.RateExpireDate != null
    //            ? Convert.ToDateTime(ext.RateExpireDate) : null;
    //        dataModel.ExternalShortTermRank = ext.ShortTermRateSymbol?.ToString();
    //        dataModel.ExternalScore = ext.Score != null ? Convert.ToDecimal(ext.Score) : null;
    //        dataModel.ExternalRankFile = ext.FileUrl;
    //        dataModel.ExternalLongTermRank = ext.LongTermRateSymbol?.ToString();
    //    }

    //    return dataModel;
    //}

    //private static LcpGuarantorRankDataModel BuildGuarantorRankDataModel(guarantordata guarantor)
    //{
    //    var dataModel = new LcpGuarantorRankDataModel();

    //    if (guarantor.IranianRanking != null)
    //    {
    //        dataModel.IranianRank = guarantor.IranianRanking.Symbol?.ToString();
    //        dataModel.IranianScore = guarantor.IranianRanking.Score != null
    //            ? Convert.ToDecimal(guarantor.IranianRanking.Score)
    //            : null;
    //    }

    //    if (guarantor.MosavabatLegalRanking != null)
    //    {
    //        var ranking = guarantor.MosavabatLegalRanking;

    //        if (ranking.LocalRanking != null)
    //        {
    //            dataModel.CreditRank = ranking.LocalRanking.Symbol?.ToString();
    //            dataModel.CreditScore = ranking.LocalRanking.Score != null
    //                ? Convert.ToDecimal(ranking.LocalRanking.Score)
    //                : null;
    //        }

    //        if (ranking.IranianRanking != null)
    //        {
    //            // نکته: در کد اصلی شما اینجا هم دوباره IranianRank/Score از
    //            // ranking.IranianRanking ست می‌شد که مقدار بالاتر (از guarantor.IranianRanking)
    //            // رو Override می‌کرد. این رفتار را عیناً حفظ کردم؛
    //            // اگر این یک باگ ناخواسته در کد اصلی بوده و نباید Override بشه، بگو تا حذفش کنم.
    //            dataModel.IranianRank = ranking.IranianRanking.Symbol?.ToString();
    //            dataModel.IranianScore = ranking.IranianRanking.Score != null
    //                ? Convert.ToDecimal(ranking.IranianRanking.Score)
    //                : null;
    //        }

    //        if (ranking.ExternalRanking != null)
    //        {
    //            var ext = ranking.ExternalRanking;
    //            dataModel.ExternalRankAssignDate = ext.RateAssignDate != null
    //                ? Convert.ToDateTime(ext.RateAssignDate) : null;
    //            dataModel.ExternalRankExpireDate = ext.RateExpireDate != null
    //                ? Convert.ToDateTime(ext.RateExpireDate) : null;

    //            // نکته: در کد اصلی اینجا به‌اشتباه .ToString() روی خود شیء
    //            // ext (نه ext.ShortTermRateSymbol) صدا زده شده بود:
    //            //   dataModel.ExternalShortTermRank = ... ext.ToString() ...
    //            // این احتمالاً یک باگ کپی/پیست بوده (برخلاف نسخه‌ی مشتری بالاتر که
    //            // درست از .ShortTermRateSymbol.ToString() استفاده می‌کنه). این‌جا
    //            // به شکل درست (ShortTermRateSymbol) اصلاحش کردم؛ اگر عمدی بوده
    //            // و می‌خوای رفتار قبلی حفظ بشه بگو تا برگردونم.
    //            dataModel.ExternalShortTermRank = ext.ShortTermRateSymbol?.ToString();
    //            dataModel.ExternalScore = ext.Score != null ? Convert.ToDecimal(ext.Score) : null;
    //            dataModel.ExternalRankFile = ext.FileUrl;
    //            dataModel.ExternalLongTermRank = ext.LongTermRateSymbol?.ToString();
    //        }
    //    }

    //    return dataModel;
    //}
}