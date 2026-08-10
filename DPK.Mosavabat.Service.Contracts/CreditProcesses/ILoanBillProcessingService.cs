using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

namespace DPK.Mosavabat.Services.Contracts.CreditProcesses;

public interface ILoanBillProcessingService
{
    Task InitiateLoanBillProcessing(
        string customerNo,
        string mosavabeNo,
        CancellationToken cancellationToken);

    Task InitiateLoanBillProcessing(
        string customerNo,
        CancellationToken cancellationToken);

    Task InitiateAllLoanBillProcessing(
        string customerNo,
        CancellationToken cancellationToken);

    Task<(List<GetMosavabeContractsDto>, List<GetLoanBillDto>)> GetProcessedContractsByMosavabeNo(string mosavabeNo,
        CancellationToken cancellationToken);

    Task<(List<GetCustomerCurrentFacilityDto>, List<GetLoanBillDto>)> GetProcessedContractsByCustomerNo(
        string customerNo,
        CancellationToken cancellationToken);

    Task<(List<GetCustomerDebtDetailsDto>, List<GetLoanBillDto>)> GetProcessedAllContractsByCustomerNo(
        string customerNo,
        CancellationToken cancellationToken);

    (List<GetCustomerCurrentFacilityDto>, List<GetLoanBillDto>)? TryGetFromCacheByCustomerNo(
        string cacheKey,
        CancellationToken cancellationToken);

    (List<GetCustomerDebtDetailsDto>, List<GetLoanBillDto>)? TryGetCustomerDebtDetailsFromCacheByCustomerNo(
        string cachekey,
        CancellationToken cancellationToken);

    (List<GetMosavabeContractsDto>, List<GetLoanBillDto>)? TryGetFromCacheByMosavabeNo(
        string cacheKey,
        CancellationToken cancellationToken);

    Task WaitForProcessingToCompleteOrTimeout(
        string customerNo,
        TimeSpan timeout,
        CancellationToken cancellationToken);
}
