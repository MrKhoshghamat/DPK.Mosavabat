using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using PaymentPermissionIssuanceControlDto = DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission.PaymentPermissionIssuanceControlDto;

namespace DPK.Mosavabat.Database.Services.Contracts.CreditProcesses
{
    public interface ICreditProcessesDatabaseService
    {
        public Task<PaymentPermissionIssuanceControlDto?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlVm viewModel, CancellationToken cancellationToken);
        public Task<CheckAndUpdateC13InPaymentPermissionDto?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetTermsAndConditionsApprovalRequestDto>?> GetTermsAndConditionsApprovalRequests(GetTermsAndConditionsApprovalRequestListVm viewModel, CancellationToken cancellationToken);
        public Task<InsertPaymentPermissionDto?> InsertPaymentPermission(InsertPaymentPermissionVm viewModel, CancellationToken cancellationToken);
        public Task<CalculateApprovalRequestUnusedBalanceDto?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceVm viewModel, CancellationToken cancellationToken);
        public Task<CalculateCustomerDebtBalanceDto?> CalculateCustomerDebtBalance(CalculateCustomerDebtBalanceVm viewModel, CancellationToken cancellationToken);
        public Task<UpdatePaymentPermissionDto?> UpdatePaymentPermission(UpdatePaymentPermissionVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetPaymentPermissionDto>?> GetPaymentPermissions(GetPaymentPermissionListVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetCuContractDto>> GetCuContracts(GetCuContractsVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetCustomerCurrentFacilityDto>> GetCustomerCurrentFacilities(GetCustomerCurrentFacilitiesVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetCustomerCurrentLetterCreditDto>> GetCustomerCurrentLetterCredits(GetCustomerCurrentLetterCreditsVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetCustomerCurrentCreditCardDto>> GetCustomerCurrentCreditCards(GetCustomerCurrentCreditCardsVm viewModel, CancellationToken cancellationToken);
        public Task<List<GetCurrentFacilityReportDto>> GetCurrentFacilitiesReport(GetCurrentFacilitiesReportVm viewModel, CancellationToken cancellationToken);
        public Task<CheckExistActivePaymentPermissionDto?> CheckExistActivePaymentPermission(CheckExistActivePaymentPermissionVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetAccNoByC13Dto> GetAccNoByC13(GetAccNoByC13Vm viewModel, CancellationToken cancellationToken = default);
        public Task<GetDateGByC13Dto> GetDateGByC13(GetDateGByC13Vm viewModel, CancellationToken cancellationToken = default);
        public Task<GetC16ByC13Dto> GetC16ByC13(GetC16ByC13Vm viewModel, CancellationToken cancellationToken = default);
        public Task<GetBranchCodeByC13Dto> GetBranchCodeByC13(GetBranchCodeByC13Vm viewModel, CancellationToken cancellationToken = default);
        public Task<CalculateLiveApprovalRequestsAmountsSumDto> CalculateLiveApprovalRequestsAmountsSum(CalculateLiveApprovalRequestsAmountsSumVm viewModel, CancellationToken cancellationToken = default);
        public Task<CalculateDailyPaidContractsAmountsSumDto> CalculateDailyPaidContractsAmountsSum(CalculateDailyPaidContractsAmountsSumVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsAmountsSumVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetLiveApprovalRequestsDto> CalculateLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsVm viewModel, CancellationToken cancellationToken = default);
        public Task<IsLcpRequestDto> IsLcpRequest(IsLcpRequestVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetListOfExceptionPayReasonDto>> GetListOfExceptionPayReason(GetListOfExceptionPayReasonVM viewModel, CancellationToken cancellationToken);
        public Task<List<GetListOfPaymentPermissionExceptionReasonDto>?> GetListOfPaymentPermissionExceptionReason(GetListOfPaymentPermissionExceptionReasonVM viewModel,
            CancellationToken cancellationToken = default);
        public Task<bool> AddToPaymentPermissionExceptionReason(PaymentPermissionExceptionReasonVM viewModel, CancellationToken cancellationToken = default);
        public Task<bool> DeletePaymentPermissionExceptionReason(PaymentPermissionExceptionReasonForDeleteVM viewModel, CancellationToken cancellationToken = default);
        public Task<GetDailyPaidContractsDto> GetDailyPaidContracts(GetDailyPaidContractsVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetCustomerContractsPastDebtDto> GetCustomerContractsPastDebt(GetCustomerContractsPastDebtVm viewModel, CancellationToken cancellationToken = default);
        public Task<List<GetListOfExceptionCodeByNationalCodeReasonDto>?> GetListOfExceptionCodeByNationalCode(GetListOfExceptionCodeByNationalCodeVM viewModel, CancellationToken cancellationToken = default);
        public Task<GetSubjectTypeDto> GetSubjectType(GetSubjectTypeVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(GetApprovalRequestInfoVm viewModel, CancellationToken cancellationToken = default);
        public Task<GetCustomerNoByMosavabeNoDto> GetCustomerNoByMosavabeNo(GetCustomerNoByMosavabeNoDataModel dataModel, CancellationToken cancellationToken = default);
        public Task<List<GetMosavabeContractsDto>> GetMosavabeContracts(GetMosavabeContractsVm viewModel, CancellationToken cancellationToken);
        public Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(IsAllowedRecalculateAccountBalanceAverageVm viewModel, CancellationToken cancellationToken);
        Task<List<ContractDebtDto>> GetContractsDebtAmounts(List<string> contractIds, CancellationToken cancellationToken);
        Task<GetRequestFacilityDetailsDto> GetRequestFacilityDetails(GetRequestFacilityDetailsVm viewModel, CancellationToken cancellationToken = default);
        Task<string?> UpdateCaseIdAndRadNumberInIssuancePayment(UpdateCaseIdAndRadNumberInIssuancePaymentVm viewModel, CancellationToken cancellationToken = default);

        

    }
}
