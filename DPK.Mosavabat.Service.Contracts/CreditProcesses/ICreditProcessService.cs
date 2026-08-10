using DPK.Mosavabat.Shared.DataTransferObjects;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using PaymentPermissionIssuanceControlDto = DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission.PaymentPermissionIssuanceControlDto;

namespace DPK.Mosavabat.Services.Contracts.CreditProcesses
{
    public interface ICreditProcessService
    {
        public Task<PaymentPermissionIssuanceControlDto?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlVm viewModel, CancellationToken cancellationToken = default);
        public Task<CheckAndUpdateC13InPaymentPermissionDto?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetTermsAndConditionsApprovalRequestDto>?> GetTermsAndConditionsApprovalRequests(GetTermsAndConditionsApprovalRequestListVm viewModel, CancellationToken cancellationToken = default);
        public Task<InsertPaymentPermissionDto?> InsertPaymentPermission(InsertPaymentPermissionVm viewModel, CancellationToken cancellationToken = default);
        public Task<CreateCompleteContractInfoProcessDto?> CreateCompleteContractInfoProcess(CreateCompleteContractInfoProcessVm viewModel, CancellationToken cancellationToken = default);
        public Task<CalculateApprovalRequestUnusedBalanceDto?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceVm viewModel, CancellationToken cancellationToken = default);
        public Task<CreateViolationInvestigationProcessDto?> CreateViolationInvestigationProcess(CreateViolationInvestigationProcessVm viewModel, CancellationToken cancellationToken = default);
        public Task<CalculateCustomerDebtBalanceDto?> CalculateCustomerDebtBalance(CalculateCustomerDebtBalanceVm viewModel, CancellationToken cancellationToken = default);
        public Task<UpdatePaymentPermissionDto?> UpdatePaymentPermission(UpdatePaymentPermissionVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetPaymentPermissionDto>?> GetPaymentPermissions(GetPaymentPermissionListVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetCuContractDto>> GetCuContracts(GetCuContractsVm viewModel, CancellationToken cancellationToken = default);
        public Task<AggregateDailyCustomerPaymentDto> AggregateDailyCustomerPayment(AggregateDailyCustomerPaymentVm viewModel, CancellationToken cancellationToken = default);
        public Task<CalculateCustomerTotalDebtDto> CalculateCustomerTotalDebt(CalculateCustomerTotalDebtVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetLoanBillDto>> GetLoanBillReport(GetLoanBillReportVm viewModel, CancellationToken cancellationToken = default);
        public Task<CheckExistActivePaymentPermissionDto?> CheckExistActivePaymentPermission(CheckExistActivePaymentPermissionVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsAmountsSumVm viewModel, CancellationToken cancellationToken = default);
        public Task<IsLcpRequestDto> IsLcpRequest(IsLcpRequestVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetListOfExceptionPayReasonDto>?> GetListOfExceptionPayReason(GetListOfExceptionPayReasonVM viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetListOfPaymentPermissionExceptionReasonDto>?> GetListOfPaymentPermissionExceptionReason(GetListOfPaymentPermissionExceptionReasonVM viewModel, CancellationToken cancellationToken = default);
        public Task<bool> AddToPaymentPermissionExceptionReason(PaymentPermissionExceptionReasonVM viewModel, CancellationToken cancellationToken = default);
        public Task<bool> DeletePaymentPermissionExceptionReason(PaymentPermissionExceptionReasonForDeleteVM viewModel, CancellationToken cancellationToken = default);
        public Task<GetDailyPaidContractsDto> GetDailyPaidContracts(GetDailyPaidContractsVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetCustomerContractsPastDebtDto> GetCustomerContractsPastDebt(GetCustomerContractsPastDebtVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetListOfExceptionCodeByNationalCodeReasonDto>?> GetListOfExceptionCodeByNationalCode(GetListOfExceptionCodeByNationalCodeVM viewModel, CancellationToken cancellationToken = default);
        public Task<GetSubjectTypeDto> GetSubjectType(GetSubjectTypeVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(GetApprovalRequestInfoVm viewModel, CancellationToken cancellationToken = default);
        public Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(IsAllowedRecalculateAccountBalanceAverageVm viewModel, CancellationToken cancellationToken = default);
    }
}
