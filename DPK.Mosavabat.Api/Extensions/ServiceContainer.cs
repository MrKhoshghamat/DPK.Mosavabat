using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.CreditProcesses;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Database.Services.Mosavabat;
using DPK.Mosavabat.External.Services.Abrisham.Loan;
using DPK.Mosavabat.External.Services.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.External.Services.Lcp;
using DPK.Mosavabat.External.Services.Payam;
using DPK.Mosavabat.Repository;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Services.Contracts.Payam;
using DPK.Mosavabat.Services.CreditProcesses;
using DPK.Mosavabat.Services.Helper;
using DPK.Mosavabat.Services.Mosavabat;
using DPK.Mosavabat.Services.Payam;

namespace DPK.Mosavabat.Api.Extensions
{
    public static class ServiceContainer
    {
        public static void ConfigServices(this IServiceCollection services)
        {
            services.AddScoped<ILoanBillProcessingService, LoanBillProcessingService>();
            services.AddScoped<ICreditProcessesDatabaseService, CreditProcessesDatabaseService>();
            services.AddScoped<IMainDatabaseService, MainDatabaseService>();
            services.AddScoped<IAccountBalanceAverageDatabaseService, AccountBalanceAverageDatabaseService>();
            services.AddScoped<IApprovalRequestDatabaseService, ApprovalRequestDatabaseService>();
            services.AddScoped<IBranchDatabaseService, BranchDatabaseService>();
            services.AddScoped<ICollateralGroupDatabaseService, CollateralGroupDatabaseService>();
            services.AddScoped<ICollateralTypeDatabaseService, CollateralTypeDatabaseService>();
            services.AddScoped<IExchangeCommercialDocDatabaseService, ExchangeCommercialDocDatabaseService>();
            services.AddScoped<IFacilityGroupDatabaseService, FacilityGroupDatabaseService>();
            services.AddScoped<IRequestFacilityDatabaseService, RequestFacilityDatabaseService>();
            services.AddScoped<ISubContractTypeDatabaseService, SubContractTypeDatabaseService>();
            services.AddScoped<IUserDatabaseService, UserDatabaseService>();
            services.AddScoped<IBeneficiariesDatabaseService, BeneficiariesDatabaseService>();
            services.AddScoped<ILoanBillRequestRepository, LoanBillRequestRepository>();
            services.AddScoped<ILoanBillDataRepository, LoanBillDataRepository>();
            services.AddScoped<ISamatExternalService, SamatExternalService>();
            services.AddScoped<IWorkflowEngineServices, WorkflowEngineServices>();
            services.AddScoped<IRequestFacilityExternalService, RequestFacilityExternalService>();
            services.AddScoped<IExternalServiceProvider, ExternalServiceProvider>();
            services.AddScoped<ILoanExternalService, LoanExternalService>();
            services.AddScoped<IBoardMemberExternalServices, BoardMemberServices>();

            services.AddScoped<ICreditProcessService, CreditProcessesService>();
            services.AddScoped<IAccountBalanceAverageService, AccountBalanceAverageService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IBoardMemberService, BoardMemberService>();
            services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IRequestFacilityService, RequestFacilityService>();
            services.AddScoped<ISamatService, SamatService>();
        }
    }
}
