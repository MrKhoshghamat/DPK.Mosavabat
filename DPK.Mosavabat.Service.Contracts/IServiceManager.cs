namespace DPK.Mosavabat.Services.Contracts;

public interface IServiceManager
{
    IAgentService AgentService { get; }
    IRequestFacilityService RequestFacilityService { get; }
    IApprovalRequestService ApprovalRequestService { get; }
    IAuthenticationService AuthenticationService { get; }
    IContractService ContractService { get; }
    IAccountBalanceAverageService AccountBalanceAverageService { get; }
    ICreditProcessesService CreditProcessesService { get; }
    ISamatService SamatService { get; }
    ILmsService  LmsService { get; }
}