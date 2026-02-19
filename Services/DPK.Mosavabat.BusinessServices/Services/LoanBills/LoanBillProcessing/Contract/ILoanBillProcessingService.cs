using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.LoanBills;

namespace DPK.Mosavabat.BusinessServices.Services.LoanBills.LoanBillProcessing.Contract;

public interface ILoanBillProcessingService
    : ITransientDependency
{
    Task InitiateLoanBillProcessing(
        string customerNo,
        string mosavabeNo,
        CancellationToken cancellationToken);

    Task InitiateLoanBillProcessing(
        string customerNo,
        CancellationToken cancellationToken);

    Task<(List<MosavabeContractsDto>, List<GetLoanBillDto>)> GetProcessedContractsByMosavabeNo(
        string mosavabeNo,
        CancellationToken cancellationToken);

    Task<(List<CustomerCurrentFacilityDto>, List<GetLoanBillDto>)> GetProcessedContractsByCustomerNo(
        string customerNo,
        CancellationToken cancellationToken);

    (List<CustomerCurrentFacilityDto>, List<GetLoanBillDto>)? TryGetFromCacheByCustomerNo(
        string cacheKey,
        CancellationToken cancellationToken);

    (List<MosavabeContractsDto>, List<GetLoanBillDto>)? TryGetFromCacheByMosavabeNo(
        string cacheKey,
        CancellationToken cancellationToken);

    Task WaitForProcessingToCompleteOrTimeout(
        string customerNo,
        TimeSpan timeout,
        CancellationToken cancellationToken);
}