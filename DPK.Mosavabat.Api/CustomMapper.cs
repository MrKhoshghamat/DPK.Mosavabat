using AutoMapper;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Dpk.UseAdanicAbrishamService.Services;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using PaymentPermissionIssuanceControlDto = DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission.PaymentPermissionIssuanceControlDto;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

namespace DPK.Mosavabat.Api
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {
            CreateMap<PaymentPermissionIssuanceControlDataModel, PaymentPermissionIssuanceControlVm>()
                .ForMember(dest => dest.MosavabeNo, opt => opt.MapFrom(src => src.MosavabeNumber)).ReverseMap();
            CreateMap<CheckAndUpdateC13InPaymentPermissionDataModel, CheckAndUpdateC13InPaymentPermissionVm>().ReverseMap();
            CreateMap<GetListTermsAndConditionsApprovalRequestDataModel, GetTermsAndConditionsApprovalRequestListVm>().ReverseMap();
            CreateMap<InsertPaymentPermissionDataModel, InsertPaymentPermissionVm>().ReverseMap();
            CreateMap<CalculateApprovalRequestUnusedBalanceDataModel, CalculateApprovalRequestUnusedBalanceVm>().ReverseMap();
            CreateMap<CreateViolationInvestigationProcessDataModel, CreateViolationInvestigationProcessVm>().ReverseMap();
            CreateMap<CalculationCustomerDebtBalanceDataModel, CalculateCustomerDebtBalanceVm>().ReverseMap();
            CreateMap<UpdatePaymentPermissionDataModel, UpdatePaymentPermissionVm>().ReverseMap();
            CreateMap<GetPaymentPermissionListDataModel, GetPaymentPermissionListVm>().ReverseMap();
            CreateMap<EstelamCheckDataModel, EstelamCheckVm>().ReverseMap();
            CreateMap<CreateCompleteContractInfoProcessDataModel, CreateCompleteContractInfoProcessVm>().ReverseMap();

            CreateMap<CheckAndUpdateC13InPaymentPermissionDataModel, CreateCompleteContractInfoProcessVm>()
                .ForMember(dest => dest.C13, opt => opt.MapFrom(src => src.C13))
                .ForMember(dest => dest.C10, opt => opt.MapFrom(src => src.CustomerNo)).ReverseMap();

            CreateMap<InsertPaymentPermissionDataModel, PaymentPermissionIssuanceControlVm>()
                .ForMember(dest => dest.CustomerSharePrice, opt => opt.MapFrom(src => src.PrepaidAmount)).ReverseMap();

            CreateMap<PaymentPermissionIssuanceControlDto, EstelamCheckVm>()
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.Result)).ReverseMap();

            CreateMap<AccountBalanceAverageDataModel, GetAccountBalanceAverageVm>().ReverseMap();
            CreateMap<GetRelatedAccountBalanceAverageDataModel, GetRelatedAccountBalanceAverageVM>().ReverseMap();
            CreateMap<ProvidingApprovedAmountFromCreditsDataModel, ProvidingApprovedAmountFromCreditsVm>().ReverseMap();

            CreateMap<MosavabatAgentDataModel, RegisterAgentVm>().ReverseMap();

            CreateMap<CreateApprovalRequestDataModel, CreateApprovalRequestVm>().ReverseMap();
            CreateMap<GetRequestStatuesDataModel, GetRequestStatusVm>().ReverseMap();

            CreateMap<GetMorabeheContractFileLinkDataModel, GetContractFileLinkVm>().ReverseMap();
            CreateMap<UpdateMorabeheContractFileLink, UpdateContractFileLinkVm>().ReverseMap();

            CreateMap<AbortCasesDataModel, AbortCasesVm>().ReverseMap();

            CreateMap<MosavabatRequestFacilityDataModel, RegisterRequestFacilityVm>()
                .ForMember(dest => dest.MosavabatRequestFacilityDataModel, opt => opt.MapFrom(src => src));
            CreateMap<MosavabatRequestFacilityUpdateDataModel, UpdateRequestFacilityVm>()
                .ForMember(dest => dest.MosavabatRequestFacilityUpdateDataModel, opt => opt.MapFrom(src => src));

            CreateMap<GetLoanInfoVm, LoanInfoVM>().ReverseMap();
            CreateMap<GetLoanBillVm, LoanBillVM>().ReverseMap();
            CreateMap<GetLoanTransactionsVm, LoanBillVM>().ReverseMap();
            CreateMap<GetLoanInstallmentCollectionTransactionsVm, LoanBillVM>().ReverseMap();
            CreateMap<GetLoanCustomerFacilitiesVm, LoanCustomerFacilityListVM>().ReverseMap();
            CreateMap<GetLoanCustomerFacilitiesDto, LoanCustomerFacilityListDto>().ReverseMap();
            CreateMap<Facility, FacilityDto>().ReverseMap();

            CreateMap<CalculateCustomerTotalDebtVm, GetLoanCustomerFacilitiesVm>()
                .ForMember(dest => dest.CustomerNo, opt => opt.MapFrom(src => src.CustomerNo));

            CreateMap<GetLoanBillReportVm, GetCurrentFacilitiesReportVm>().ReverseMap();
            CreateMap<GetLoanBillDto, LoanBillDto>().ReverseMap();
            CreateMap<Transactions, TransactionDto>();
            CreateMap<AdanicApiBaseResultDto, AbrishamResultDto>();

            CreateMap<GetSettledLoanBillVm, LoanBillVM>().ReverseMap();
            CreateMap<GetPayedLoanBillVm, LoanBillVM>().ReverseMap();
            CreateMap<GetReceivedLoanBillVm, LoanBillVM>().ReverseMap();

            CreateMap<List<Transactions>, GetSettledLoanBillDto>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src));

            CreateMap<List<Transactions>, GetPayedLoanBillDto>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src));

            CreateMap<List<Transactions>, GetReceivedLoanBillDto>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src));

            CreateMap<AggregateDailyCustomerPaymentVm, GetCustomerCurrentFacilitiesVm>().ReverseMap();
            CreateMap<AggregateDailyCustomerPaymentVm, GetCustomerCurrentLetterCreditsVm>().ReverseMap();
            CreateMap<AggregateDailyCustomerPaymentVm, GetCustomerCurrentCreditCardsVm>().ReverseMap();

            CreateMap<CheckExistActivePaymentPermissionDataModel, CheckExistActivePaymentPermissionVm>().ReverseMap();

            CreateMap<GetApprovalContractsVm, GetApprovalContractsDm>().ReverseMap();

            CreateMap<GetLoanBillDto, GetLoanBillDm>().ReverseMap();
            CreateMap<TransactionDto, TransactionDm>().ReverseMap();

            CreateMap<GetContractsVm, GetCuContractsVm>().ReverseMap();
            CreateMap<GetContractsVm, GetPaymentPermissionListDataModel>()
                .ForMember(dest => dest.MosavabeNumber, opt => opt.MapFrom(src => src.MosavabeNo)).ReverseMap();

            CreateMap<GetPaymentPermissionDto, GetContractsDto>();
            CreateMap<GetCuContractDto, GetContractsDto>();

            CreateMap<GetPaymentPermissionDto, GetCuContractDto>().ReverseMap();
            CreateMap<GetPaymentPermissionListDataModel, GetCuContractsVm>()
                .ForMember(dest => dest.MosavabeNo, opt => opt.MapFrom(src => src.MosavabeNumber)).ReverseMap();


            CreateMap<CalculateCustomerTotalDebtVm, CalculateLiveApprovalRequestsAmountsSumVm>()
                .ForMember(dest => dest.CustNo, opt => opt.MapFrom(src => src.CustomerNo)).ReverseMap();

            CreateMap<CalculateCustomerTotalDebtVm, CalculateDailyPaidContractsAmountsSumVm>()
                .ForMember(dest => dest.CustNo, opt => opt.MapFrom(src => src.CustomerNo)).ReverseMap();

            CreateMap<FindRegionAndBrancheBySupervisorDataModel, FindRegionAndBrancheBySupervisorVm>().ReverseMap();

            CreateMap<GetContractTypeTitleByContractTypeIdVm, GetContractTypeTitleByContractTypeIdDm>().ReverseMap();
            CreateMap<GetSubContractTypeTitleBySubContractTypeIdVm, GetSubContractTypeTitleBySubContractTypeIdDm>().ReverseMap();

            CreateMap<UpdateLcpRequestRadNumberVm, UpdateLcpRequestRadNumberDataModel>().ReverseMap();
            CreateMap<GetBeneficiariesDebtListVm, GetBeneficiariesDebtListDataModel>().ReverseMap();
            CreateMap<IsRelatedPersonVm, IsRelatedPersonDataModel>().ReverseMap();
            CreateMap<GetAccountBalanceAverageTarhJariZendegiDataModel, GetAccountBalanceAverageTarhJariZendegiVm>().ReverseMap();

            CreateMap<GetLoanContractsAllStatementVm, LoanContractsAllStatementVM>().ReverseMap();
            CreateMap<TransactionInformationDto, TransactionInfoDto>();
            CreateMap<LoanContractsAllStatementDto, GetLoanContractsAllStatementDto>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.TransactionInformationDtos));

            CreateMap<TransactionInfoDto, TransactionDto>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src =>
                    (src.Date).Replace("/", "").Trim()))
                .ForMember(dest => dest.PaymentTime, opt => opt.MapFrom(src =>
                    (src.Time).Trim()))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src =>
                    (src.TransactionAmount).Replace(",", "").Trim()))
                .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src =>
                    (src.BranchNo).Trim()))
                .ForMember(dest => dest.TransactionCode, opt => opt.MapFrom(src =>
                    (src.TrCd).Trim()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.Description1) ? string.Empty : src.Description1.Trim()))
                .ForMember(dest => dest.PaymentGateway, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.User) ? null : src.User.Trim()))
                .ForMember(dest => dest.Indicator, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.ReversedTransactionDocumentRow) ? "0" : "1"));


            CreateMap<GetLoanContractsAllStatementDto, GetLoanBillDto>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions))
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src =>
                    src.Transactions.Where(t =>
                    !string.IsNullOrWhiteSpace(t.Date))));

            CreateMap<GetLoanContractsAllStatementDto, GetLoanBillDto>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions))
                .ForMember(dest => dest.ProgramName, opt => opt.Ignore())
                .ForMember(dest => dest.LoanId, opt => opt.Ignore())
                .ForMember(dest => dest.BillType, opt => opt.Ignore())
                .ForMember(dest => dest.Terminal, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPhases, opt => opt.Ignore())
                .ForMember(dest => dest.Phase, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterPaymentPermissionExceptionReasonVm, RegisterPaymentPermissionExceptionReasonsDataModel>().ReverseMap();
            CreateMap<GetPaymentPermissionExceptionReasonsVm, GetPaymentPermissionExceptionReasonsDataModel>().ReverseMap();
            CreateMap<CalculateRequestFacilityUnusedBalanceDto,CalculateApprovalRequestUnusedBalanceDto>().ReverseMap();

        }
    }
}
