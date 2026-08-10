using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Entities.Models;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.External.Services.Contracts.Lms;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DPK.Mosavabat.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IAgentService> _agentService;
    private readonly Lazy<IApprovalRequestService> _approvalRequestService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IContractService> _contractService;
    private readonly Lazy<IRequestFacilityService> _requestFacilityService;
    private readonly Lazy<IAccountBalanceAverageService> _accountBalanceAverageService;
    private readonly Lazy<ICreditProcessesService> _creditProcessesService;
    private readonly Lazy<ISamatService> _samatService;
    private readonly ILmsService _lmsService;

    public ServiceManager
    (
        IRepositoryManager repositoryManager,
        ILoggerManager logger,
        ILmsServiceProxy lmsServiceProxy,
        IMapper mapper,
        IWorkflowEngineServices workflowEngineServices,
        ISamatServices samatServices,
        ICollateralTypeDatabase collateralTypeDatabase,
        IRequestFacilityDatabase requestFacilityDatabase,
        IApprovalRequestDatabase approvalRequestDatabase,
        IAccountBalanceAverageDatabase accountBalanceAverageDatabase,
        IBranchDatabase branchDatabase,
        IRequestFacilityServices requestFacilityServices,
        IUserDatabase userDatabase,
        ICreditProcessesDatabase creditProcessesDatabase,
        UserManager<User> userManager,
        IConfiguration configuration,
        IExchangeCommercialDocDatabase exchangeCommercialDocDatabase,
        IFacilityGroupDatabase facilityGroupDatabase,
        ISubContractTypeDatabase subContractTypeDatabase,
        ILmsService lmsService)
    {
        _lmsService = lmsService;
        _agentService = new Lazy<IAgentService>(() => new AgentService(logger, workflowEngineServices, userDatabase));
        _approvalRequestService = new Lazy<IApprovalRequestService>(() => new ApprovalRequestService(logger, approvalRequestDatabase, workflowEngineServices, userDatabase, requestFacilityServices, requestFacilityDatabase, branchDatabase));
        _requestFacilityService = new Lazy<IRequestFacilityService>(() => new RequestFacilityService(logger, repositoryManager, workflowEngineServices, mapper, userDatabase, requestFacilityDatabase, collateralTypeDatabase, exchangeCommercialDocDatabase, facilityGroupDatabase, subContractTypeDatabase));
        _accountBalanceAverageService = new Lazy<IAccountBalanceAverageService>(() => new AccountBalanceAverageService(logger, accountBalanceAverageDatabase));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
        _contractService = new Lazy<IContractService>(() => new ContractService(logger, requestFacilityDatabase));
        _creditProcessesService = new Lazy<ICreditProcessesService>(() => new CreditProcessesService(logger, creditProcessesDatabase, workflowEngineServices, userDatabase));
        _samatService = new Lazy<ISamatService>(() => new SamatService(logger, samatServices));
    }

    public IAgentService AgentService => _agentService.Value;
    public IRequestFacilityService RequestFacilityService => _requestFacilityService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IContractService ContractService => _contractService.Value;
    public IApprovalRequestService ApprovalRequestService => _approvalRequestService.Value;
    public IAccountBalanceAverageService AccountBalanceAverageService => _accountBalanceAverageService.Value;
    public ICreditProcessesService CreditProcessesService => _creditProcessesService.Value;
    public ISamatService SamatService => _samatService.Value;
    public ILmsService LmsService => _lmsService;
}