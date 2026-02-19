using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace DPK.Mosavabat.DomainEntities.LoanBill.Repositories;

public interface ILoanBillRequestRepository
{
    Task AddRangeAsync(
        List<LoanBillRequest> requests,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        LoanBillRequest request,
        CancellationToken cancellationToken);

    Task<List<LoanBillRequest>> GetPendingRequestsAsync(
        int takeCount,
        CancellationToken cancellationToken);

    Task<List<LoanBillRequest>> GetActiveRequestsByCustomerAsync(
        string customerNo,
        DateTime cutoffUtcTime,
        CancellationToken cancellationToken);

    Task<bool> GetIsProcessingRequestsAsync(
        string customerNo,
        DateTime cutOffUtcTime,
        CancellationToken cancellationToken);
}