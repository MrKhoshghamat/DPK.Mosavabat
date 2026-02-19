using DPK.Foundation.Identity.Models.Core;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.DataModels.Payam;
using DPK.Mosavabat.Models.DataModels.Proxies.Lcp;
using DPK.Mosavabat.Models.DataModels.Proxies.Lms;
using DPK.Mosavabat.Models.EntitiesDto.Account;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.LoanBills;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.Models.ViewModels.CreditProcesses;

namespace DPK.Mosavabat.Models;

internal static class CustomDtoMapper
{

    public static void CreateMappings(IMapperConfigurationExpression configuration)
    {
        ConfigModelName(configuration);
        ConfigLoanBill(configuration);
        ConfigCreditProcessesForApplicationServices(configuration);
        ConfigCreditProcessesForExternalServices(configuration);
        ConfigLcpForExternalServices(configuration);
    }

    private static void ConfigModelName(IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<User, UserListDto>();
    }

    private static void ConfigLoanBill(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<GetLoanBillDto, GetLoanBillResultDm>().ReverseMap();
        cfg.CreateMap<TransactionDto, TransactionDm>().ReverseMap();
    }

    private static void ConfigCreditProcessesForApplicationServices(IMapperConfigurationExpression cfg)
    {
        // Data Transfer Objects to Data Models
        cfg.CreateMap<CustomerCurrentFacilityDto, GetCustomerCurrentFacilityDm>().ReverseMap();

        // Commands to Data Models
        cfg.CreateMap<ControlPaymentPermissionIssuanceCommand, ControlPaymentPermissionIssuanceDm>().ReverseMap();
        cfg.CreateMap<InsertPaymentPermissionCommand, InsertPaymentPermissionDm>().ReverseMap();
        cfg.CreateMap<UpdatePaymentPermissionCommand, UpdatePaymentPermissionDm>().ReverseMap();

        // Queries to Data Models
        cfg.CreateMap<GetApprovalRequestInfoQuery, GetApprovalRequestInfoDm>().ReverseMap();
        cfg.CreateMap<GetApprovalRequestTermsAndConditionsQuery, GetApprovalRequestTermsAndConditionsDm>().ReverseMap();
        cfg.CreateMap<GetCheckInquiryQuery, CheckInquiryDm>().ReverseMap();
        cfg.CreateMap<GetContractsQuery, FetchPaymentPermissionsDm>().ReverseMap();
        cfg.CreateMap<GetContractsQuery, GetCuContractsDm>().ReverseMap();
        cfg.CreateMap<GetCustomerContractsPastDebtsQuery, GetCustomerContractsPastDebtsDm>().ReverseMap();
        cfg.CreateMap<GetDailyPaidContractsQuery, GetCustomerDailyPaidContractsDm>().ReverseMap();
        cfg.CreateMap<GetLiveApprovalRequestsAmountsSumQuery, GetLiveApprovalRequestsAmountsSumDm>().ReverseMap();
        cfg.CreateMap<GetPaymentPermissionsQuery, FetchPaymentPermissionsDm>().ReverseMap();
        cfg.CreateMap<GetSubjectTypeByMosavabeNoQuery, GetSubjectTypeByMosavabeNoDm>().ReverseMap();
        cfg.CreateMap<IsAllowedRecalculateAccountBalanceAverageQuery, IsAllowedRecalculateAccountBalanceAverageDm>().ReverseMap();
        cfg.CreateMap<IsExistActivePaymentPermissionQuery, IsExistActivePaymentPermissionDm>().ReverseMap();
        cfg.CreateMap<IsLcpRequestQuery, IsLcpRequestDm>().ReverseMap();

        // View Models to Commands
        cfg.CreateMap<AggregateDailyCustomerPaymentVm, AggregateDailyCustomerPaymentCommand>().ReverseMap();
        cfg.CreateMap<CalculateApprovalRequestUnusedBalanceVm, CalculateApprovalRequestUnusedBalanceCommand>().ReverseMap();
        cfg.CreateMap<CalculateCustomerTotalDebtVm, CalculateCustomerTotalDebtCommand>().ReverseMap();
        cfg.CreateMap<ControlPaymentPermissionIssuanceVm, ControlPaymentPermissionIssuanceCommand>().ReverseMap();
        cfg.CreateMap<InsertPaymentPermissionVm, InsertPaymentPermissionCommand>().ReverseMap();
        cfg.CreateMap<UpdatePaymentPermissionVm, UpdatePaymentPermissionCommand>().ReverseMap();
        cfg.CreateMap<CreateCompleteContractInfoProcessVm, CreateCompleteContractInfoProcessCommand>().ReverseMap();

        // View Models to Queries
        cfg.CreateMap<GetApprovalRequestInfoVm, GetApprovalRequestInfoQuery>().ReverseMap();
        cfg.CreateMap<GetApprovalRequestTermsAndConditionsVm, GetApprovalRequestTermsAndConditionsQuery>().ReverseMap();
        cfg.CreateMap<GetContractsVm, GetContractsQuery>().ReverseMap();
        cfg.CreateMap<GetCustomerContractsPastDebtsVm, GetCustomerContractsPastDebtsQuery>().ReverseMap();
        cfg.CreateMap<GetDailyPaidContractsVm, GetDailyPaidContractsQuery>().ReverseMap();
        cfg.CreateMap<GetLiveApprovalRequestsAmountsSumVm, GetLiveApprovalRequestsAmountsSumQuery>().ReverseMap();
        cfg.CreateMap<GetPaymentPermissionsVm, GetPaymentPermissionsQuery>().ReverseMap();
        cfg.CreateMap<GetSubjectTypeByMosavabeNoVm, GetSubjectTypeByMosavabeNoQuery>().ReverseMap();
        cfg.CreateMap<IsAllowedRecalculateAccountBalanceAverageVm, IsAllowedRecalculateAccountBalanceAverageQuery>().ReverseMap();
        cfg.CreateMap<IsExistActivePaymentPermissionVm, IsExistActivePaymentPermissionQuery>().ReverseMap();
        cfg.CreateMap<IsLcpRequestVm, IsLcpRequestQuery>().ReverseMap();
        cfg.CreateMap<GetCheckInquiryVm, GetCheckInquiryQuery>().ReverseMap();
    }

    private static void ConfigCreditProcessesForExternalServices(IMapperConfigurationExpression cfg)
    {
        // Data Models to Data Models
        cfg.CreateMap<InsertPaymentPermissionDm, InsertPaymentPermissionProxyDm>().ReverseMap();
        cfg.CreateMap<UpdatePaymentPermissionDm, UpdatePaymentPermissionProxyDm>().ReverseMap();
        cfg.CreateMap<FetchPaymentPermissionsDm, FetchPaymentPermissionsProxyDm>().ReverseMap();
    }

    private static void ConfigLcpForExternalServices(IMapperConfigurationExpression cfg)
    {
        // Data Models to Data Models
        cfg.CreateMap<AcceptAgentDm, AcceptAgentProxyDm>().ReverseMap();
        cfg.CreateMap<CreateTicketDm, CreateTicketProxyDm>().ReverseMap();
        cfg.CreateMap<GetBoardMembersDm, GetBoardMembersProxyDm>().ReverseMap();
        cfg.CreateMap<UpdateLcpRequestFacilityRadNumberDm, UpdateLcpRequestFacilityRadNumberProxyDm>().ReverseMap();
        cfg.CreateMap<UpdateLcpRequestFacilityStatusDm, UpdateLcpRequestFacilityStatusProxyDm>().ReverseMap();
    }
}