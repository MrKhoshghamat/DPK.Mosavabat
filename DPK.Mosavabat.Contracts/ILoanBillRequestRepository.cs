using DPK.Mosavabat.Entities.Models.LoanBill;

namespace DPK.Mosavabat.Contracts;

public interface ILoanBillRequestRepository : IRepositoryBase<LoanBillRequest>
{
    Task AddRangeAsync(List<LoanBillRequest> requests, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task UpdateAsync(LoanBillRequest request, CancellationToken cancellationToken);
    Task<List<LoanBillRequest>> GetPendingRequestsAsync(int takeCount, CancellationToken cancellationToken);
}