using System.Xml;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Mosavabat;

public class ApprovalRequestService : IApprovalRequestService
{
    private readonly IApprovalRequestDatabaseService _approvalRequestDatabaseService;
    private readonly IRequestFacilityExternalService _requestFacilityExternalService;
    private readonly IRequestFacilityDatabaseService _requestFacilityDatabaseService;
    private readonly IUserDatabaseService _userDatabaseService;
    private readonly IWorkflowEngineServices _workflowEngineServices;
    private readonly IBranchDatabaseService _branchDatabaseService;
    private readonly ILoggerManager _logger;

    public ApprovalRequestService(
        IApprovalRequestDatabaseService approvalRequestDatabaseService,
        IRequestFacilityExternalService requestFacilityExternalService,
        IRequestFacilityDatabaseService requestFacilityDatabaseService,
        IUserDatabaseService userDatabaseService,
        IWorkflowEngineServices workflowEngineServices,
        IBranchDatabaseService branchDatabaseService,
        ILoggerManager logger)
    {
        _approvalRequestDatabaseService = approvalRequestDatabaseService;
        _requestFacilityExternalService = requestFacilityExternalService;
        _requestFacilityDatabaseService = requestFacilityDatabaseService;
        _userDatabaseService = userDatabaseService;
        _workflowEngineServices = workflowEngineServices;
        _branchDatabaseService = branchDatabaseService;
        _logger = logger;
    }

    public async Task<List<GetRequestStatusDto>?> GetRequestStatus(GetRequestStatusVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _approvalRequestDatabaseService.GetRequestStatus(viewModel, cancellationToken);
    }

    public async Task<CreateApprovalRequestDto?> CreateApprovalRequest(CreateApprovalRequestVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var getCustomerRankDto = await _requestFacilityDatabaseService.GetCustomerRank(
            viewModel.LcpRequestFacilityId,
            cancellationToken);

        var response = await CreateApprovalRequest(
            viewModel.LcpRequestFacilityId, 
            getCustomerRankDto.CreditScore,
            getCustomerRankDto.CreditRank!,
            cancellationToken);

        switch (response)
        {
            case { CaseId: 0 or <= 0 }:
                return new CreateApprovalRequestDto() { MosavabatResponseDataModel = response };
            case null:
                return new CreateApprovalRequestDto()
                { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };
        }

        await _requestFacilityExternalService.UpdateRadNumberRequestFacility(new UpdateRequestFacilityRadNumberVm()
        {
            CaseId = viewModel.LcpRequestFacilityIdCase,
            RadNumber = response.RadNumber,
            CaseIdApprovalRequest = response.CaseId,
        }, cancellationToken);

        await _requestFacilityDatabaseService.UpdateLcpRequestFacility(new UpdateLcpRequestFacilityVm()
        {
            IdCase = viewModel.LcpRequestFacilityIdCase,
            InitialRadNumber = viewModel.LcpRequestFacilityRadNumber,
            IdCaseApprovalRequest = response.CaseId,
            RadNumberApprovalRequest = response.RadNumber,
            LcpRequestFacilityId = viewModel.LcpRequestFacilityId,
            GuidLcpRequestFacility = viewModel.GuidLcpRequestFacility
        }, cancellationToken);

        await _requestFacilityDatabaseService.MapLcpRequestFacilityToApprovalRequest(new MapLcpRequestFacilityToApprovalRequestVm()
        {
            IdCase = viewModel.LcpRequestFacilityIdCase,
            IdCaseApprovalRequest = response.CaseId,
            IdLcpRequestFacility = viewModel.LcpRequestFacilityId
        }, cancellationToken);

        return new CreateApprovalRequestDto() { MosavabatResponseDataModel = response };
    }

    private async Task<MosavabatResponseDataModel?> CreateApprovalRequest(
        int lcpRequestFacilityId,
        float? creditScore,
        string creditRank,
        CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(lcpRequestFacilityId));
        var approvalsRequest = new MosavabatApprovalsRequest()
        {
            ApprovalRequestGroup = "1",
            RequestTypeDetail = "1",
            CreditScore = creditScore,
            CreditRank = creditRank
        };

        var branchCode = await _branchDatabaseService.GetBranchCode(new GetBranchCodeVm() { IdLcpRequestFacility = lcpRequestFacilityId }, cancellationToken);

        var getUserNameResult = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = Convert.ToInt32(branchCode.BranchCode),
            MosavabatClassName = MosavabatClassName.ApprovalRequest2
        }, cancellationToken);

        if (string.IsNullOrEmpty(getUserNameResult.Username))
            return new MosavabatResponseDataModel(default, default);

        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(approvalsRequest, getUserNameResult.Username, MosavabatClassName.ApprovalRequest2.ToString());

        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "approvalsRequest");

        var xDoc = new XmlDocument();

        xDoc.LoadXml(data);

        return await _workflowEngineServices.CreateCasesAsync(xDoc);
    }

    public async Task<IsExistFacilitySubTypeCodeInGroupCreditPackageDto> IsExistFacilitySubTypeCodeInGroupCreditPackage(IsExistFacilitySubTypeCodeInGroupCreditPackageVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _approvalRequestDatabaseService.IsExistFacilitySubTypeCodeInGroupCreditPackage(viewModel, cancellationToken);
    }

    public async Task<GetRequestsLastStatusDto?> GetRequestsLastStatus(
        GetRequestsLastStatusVm viewModel,
        CancellationToken cancellationToken = default)
    {
        var results = await _approvalRequestDatabaseService.GetRequestsLastStatusBulk(
            viewModel.CaseIds,
            cancellationToken);

        var statuses = results.ToDictionary(
            r => r.CaseId,
            r => r.StatusDescription);

        return new GetRequestsLastStatusDto { Statuses = statuses };
    }
}