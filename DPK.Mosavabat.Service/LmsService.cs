using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Lms;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Services.Helper;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using Newtonsoft.Json;
using System.Xml;

namespace DPK.Mosavabat.Services
{
    public class LmsService : ILmsService
    {
        private readonly ILmsServiceProxy _lmsServiceProxy;
        private readonly IWorkflowEngineServices _workflowEngine;
        private readonly ICreditProcessesDatabaseService _creditProcessesDatabaseService;
        private readonly ILoggerManager _logger;

        public LmsService(
            ILmsServiceProxy lmsServiceProxy,
            IWorkflowEngineServices workflowEngine,
            ICreditProcessesDatabaseService creditProcessesDatabaseService,
            ILoggerManager logger)
        {
            _lmsServiceProxy = lmsServiceProxy;
            _workflowEngine = workflowEngine;
            _creditProcessesDatabaseService = creditProcessesDatabaseService;
            _logger = logger;
        }

        public Task<BaseResponse<LmsResponse>> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return _lmsServiceProxy.CheckAndUpdateC13InPaymentPermission(dataModel);

        }

        public Task<BaseResponse<LmsResponse>> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return _lmsServiceProxy.InsertPaymentPermission(dataModel);

        }

        public Task<BaseResponse<LmsResponse>> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return _lmsServiceProxy.UpdatePaymentPermission(dataModel);

        }

        public Task<BaseResponse<List<GetPaymentPermissionDto>>> FetchPaymentPermissions(GetPaymentPermissionListDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return _lmsServiceProxy.FetchPaymentPermissions(dataModel);
        }

        public Task<GetIsCanceledPaymentPermissionsDto> FetchIsCanceledPaymentPermissions()
        {
            _logger.Trace(JsonConvert.SerializeObject("FetchIsCanceledPaymentPermissions request."));
            return _lmsServiceProxy.FetchIsCanceledPaymentPermissions();

        }

        public Task<BaseResponse<LmsResponse>> RegisterPaymentPermissionExceptionReason(RegisterPaymentPermissionExceptionReasonsDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject("RegisterPaymentPermissionExceptionReason"));
            return _lmsServiceProxy.RegisterPaymentPermissionExceptionReason(dataModel);
        }

        public Task<BaseResponse<List<GetPaymentPermissionExceptionReasonsDto>>> GetPaymentPermissionExceptionReasons(GetPaymentPermissionExceptionReasonsDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject("GetPaymentPermissionExceptionReasons"));
            return _lmsServiceProxy.GetPaymentPermissionExceptionReasons(dataModel);
        }


        public Task<BaseResponse<BoundaryComplianceInquiryDto>> CheckBoundaryComplianceInquiry(BoundaryComplianceInquiryDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            return _lmsServiceProxy.CheckBoundaryComplianceInquiry(dataModel);
        }

        public async Task<MosavabatResponseDataModel?> CreateIssuancePayment(
            CreateIssuancePaymentPermissionDataModel dataModel)
        {
            // 1 — Build the domain model from the input DTO
            var model = BuildIssuancePaymentModel(dataModel);

            // 2 — Map child collections
            MapGuarantors(dataModel, model);
            MapCollaterals(dataModel, model);
            MapFactors(dataModel, model);
            MapBeneficiaries(dataModel, model);

            // 3 — Create the XML document (only when UserName is present)
            var xmlDocument = CreateXmlDocument(model, dataModel.UserName, "IssuancePeymentId");

            // 4 — Send to workflow engine
            var response = await _workflowEngine.CreateCasesAsync(xmlDocument);

            // 5 — If no case was created, exit early
            if (response is null || response.CaseId == 0)
                return default;

            // 6 — Persist CaseId and RadNumber
            await _creditProcessesDatabaseService.UpdateCaseIdAndRadNumberInIssuancePayment(
                new UpdateCaseIdAndRadNumberInIssuancePaymentVm
                {
                    CaseId = response.CaseId,
                    RadNumber = response.RadNumber.ToString(),
                    Guid = model.GUID
                });

            return response;
        }

        private static IssuancePaymentPermissionDataModel BuildIssuancePaymentModel(
            CreateIssuancePaymentPermissionDataModel src)
        {
            return new IssuancePaymentPermissionDataModel
            {
                GUID = Guid.NewGuid().ToString(),
                Creator = src.UserId,
                CUApprovalSubject = src.MosavabeNo,
                Request_Date = DateTime.Now,
                CU_Customer = Convert.ToInt32(src.Customer),
                Branches = src.BranchId,
                ContractAmount = src.ContractAmount,
                BankSharePrice = src.BankSharePrice,
                CustomerCashPrice = src.CustomerCashPrice,
                CustomerNonCashPrice = src.CustomerNonCashPrice,
                CreditRank = src.CreditRank,
                CreditScore = src.CreditScore,
                LcpId = src.LcpId,
                Description = src.Description,
                BaratDueDate = src.BaratDueDate,
                FeeAmount = src.FeeAmount,
                Beneficiaries = new PayBaratBeneficiaries() { Beneficiaries = new List<PayBaratBeneficiary>() },
                Factors = new PayBaratFactors() { Factors = new List<PayBaratFactor>() },
                Collaterals = new PayCollaterals() { Collaterals = new List<PayCollateral>() },
                Guarantors = new PayGuarantors() { Guarantors = new List<PayGuarantor>() }
            };
        }

        private static void MapGuarantors(
            CreateIssuancePaymentPermissionDataModel src,
            IssuancePaymentPermissionDataModel dest)
        {
            foreach (var g in src.Guarantors.Guarantors)
            {
                dest.Guarantors.Guarantors.Add(new PayGuarantor
                {
                    Nationality = g.Nationality,
                    Country = g.Country,
                    CreditRank = g.CreditRank,
                    CreditScore = g.CreditScore,
                    Description = g.Description,
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    GuarantorType = g.GuarantorType,
                    LcpId = g.LcpId.ToString(),
                    NationalCode = g.NationalCode
                });
            }
        }

        private static void MapCollaterals(
            CreateIssuancePaymentPermissionDataModel src,
            IssuancePaymentPermissionDataModel dest)
        {
            foreach (var c in src.Collaterals.Collaterals)
            {
                dest.Collaterals.Collaterals.Add(new PayCollateral
                {
                    Amount = c.Amount,
                    CollateralType = c.CollateralType,
                    CoverPercent = c.CoverPercent,
                    Description = c.Description,
                    LcpId = c.LcpId.ToString()
                });
            }
        }

        private static void MapFactors(
            CreateIssuancePaymentPermissionDataModel src,
            IssuancePaymentPermissionDataModel dest)
        {
            foreach (var f in src.Factors.Factors)
            {
                dest.Factors.Factors.Add(new PayBaratFactor
                {
                    FactorId = f.FactorId,
                    LcpId = f.LcpId
                });
            }
        }

        private static void MapBeneficiaries(
            CreateIssuancePaymentPermissionDataModel src,
            IssuancePaymentPermissionDataModel dest)
        {
            foreach (var b in src.Beneficiaries.Beneficiaries)
            {
                dest.Beneficiaries.Beneficiaries.Add(new PayBaratBeneficiary
                {
                    LcpId = b.LcpId.ToString(),
                    BeneficiaryType = b.BeneficiaryType,
                    Iban = b.Iban,
                    LastName = b.LastName,
                    Name = b.Name,
                    NationalCode = b.NationalCode,
                    ShahabId = b.ShahabId
                });
            }
        }

        private static XmlDocument CreateXmlDocument(
            IssuancePaymentPermissionDataModel model,
            string? userName,
            string entityId)
        {
            var xmlDocument = new XmlDocument();

            if (userName is null)
                return xmlDocument;

            var standardModel = BizagiHelpers.CreateStandardModelForBizagi(model, userName, entityId);
            var data = BizagiHelpers.ConvertModelToXmlMosavabat(standardModel, entityId);
            xmlDocument.LoadXml(data);

            return xmlDocument;
        }
    }
}
