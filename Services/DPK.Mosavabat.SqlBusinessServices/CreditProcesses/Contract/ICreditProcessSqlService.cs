using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;

namespace DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

public interface ICreditProcessSqlService
    : ITransientDependency
{
    Task<List<MosavabeContractsDto>> GetMosavabeContracts(
        GetMosavabeContractsDm dm,
        CancellationToken cancellationToken);

    Task<List<CustomerCurrentFacilityDto>> GetCustomerCurrentFacilities(
        GetCustomerCurrentFacilitiesDm dm,
        CancellationToken cancellationToken);

    Task<GetCustomerNoByMosavabeNoDto> GetCustomerNoByMosavabeNo(
        GetCustomerNoByMosavabeNoDm dm,
        CancellationToken cancellationToken);

    Task<CalculateApprovalRequestUnusedBalanceDto> CalculateApprovalRequestUnusedBalance(
        CalculateApprovalRequestUnusedBalanceDm dm,
        CancellationToken cancellationToken);

    Task<CalculateDailyPaidContractsAmountsSumDto> CalculateDailyPaidContractsAmountsSum(
        CalculateDailyPaidContractsAmountsSumDm dm,
        CancellationToken cancellationToken);

    Task<ControlPaymentPermissionIssuanceDto> ControlPaymentPermissionIssuance(
        ControlPaymentPermissionIssuanceDm dm,
        CancellationToken cancellationToken);

    Task<UpdateIsDeletedCuContractDto> UpdateIsDeletedCuContract(
        UpdateIsDeletedCuContractDm dm,
        CancellationToken cancellationToken);

    Task<GetBranchCodeByC13Dto> GetBranchCodeByC13(
        GetBranchCodeByC13Dm dm,
        CancellationToken cancellationToken);

    Task<GetAccNoByC13Dto> GetAccNoByC13(
        GetAccNoByC13Dm dm,
        CancellationToken cancellationToken);

    Task<GetDateGByC13Dto> GetDateGByC13(
        GetDateGByC13Dm dm,
        CancellationToken cancellationToken);

    Task<GetC16ByC13Dto> GetC16ByC13(
        GetC16ByC13Dm dm,
        CancellationToken cancellationToken);

    Task<GetCustomerEkDto> GetCustomerEk(
        GetCustomerEkDm dm,
        CancellationToken cancellationToken);

    Task<UpdateCuCustomerInCuContractDto> UpdateCuCustomerInCuContract(
        UpdateCuCustomerInCuContractDm dm,
        CancellationToken cancellationToken);

    Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(
        GetApprovalRequestInfoDm dm,
        CancellationToken cancellationToken);

    Task<List<ApprovalRequestTermsAndConditionsDto>> GetApprovalRequestTermsAndConditions(
        GetApprovalRequestTermsAndConditionsDm dm,
        CancellationToken cancellationToken);

    Task<List<CuContractDto>> GetCuContracts(
        GetCuContractsDm dm,
        CancellationToken cancellationToken);

    Task<List<CustomerPastDebtDto>> GetCustomerContractsPastDebts(
        GetCustomerContractsPastDebtsDm dm,
        CancellationToken cancellationToken);

    Task<List<CustomerDailyPaidContractDto>> GetCustomerDailyPaidContracts(
        GetCustomerDailyPaidContractsDm dm,
        CancellationToken cancellationToken);

    Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(
        GetLiveApprovalRequestsAmountsSumDm dm,
        CancellationToken cancellationToken);

    Task<GetSubjectTypeByMosavabeNoDto> GetSubjectTypeByMosavabeNo(
        GetSubjectTypeByMosavabeNoDm dm,
        CancellationToken cancellationToken);

    Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(
        IsAllowedRecalculateAccountBalanceAverageDm dm,
        CancellationToken cancellationToken);

    Task<IsExistActivePaymentPermissionDto> IsExistActivePaymentPermission(
        IsExistActivePaymentPermissionDm dm,
        CancellationToken cancellationToken);

    Task<IsLcpRequestDto> IsLcpRequest(
        IsLcpRequestDm dm,
        CancellationToken cancellationToken);
}