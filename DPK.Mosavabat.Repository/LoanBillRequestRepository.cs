using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Entities.Helper;
using DPK.Mosavabat.Entities.Models.LoanBill;
using Microsoft.EntityFrameworkCore;

namespace DPK.Mosavabat.Repository;

public class LoanBillRequestRepository : RepositoryBase<LoanBillRequest>, ILoanBillRequestRepository
{
    private readonly RepositoryContext _repositoryContext;

    public LoanBillRequestRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public async Task AddRangeAsync(List<LoanBillRequest> requests, CancellationToken cancellationToken)
    {
        await _repositoryContext.LoanBillRequests.AddRangeAsync(requests, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _repositoryContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(LoanBillRequest request, CancellationToken cancellationToken)
    {
        _repositoryContext.Entry(request).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task<List<LoanBillRequest>> GetPendingRequestsAsync(
        int takeCount,
        CancellationToken cancellationToken)
    {
        var cutoffIranTime = IranTimeHelper
            .ToIranTimeNow()
            .AddMinutes(-10);

        var cutoffUtcTime = TimeZoneInfo
            .ConvertTimeToUtc(
                cutoffIranTime,
                IranTimeHelper.IranTimeZone);

        return await _repositoryContext.LoanBillRequests
            .Where(r =>
                    !r.IsCompleted &&
                    !r.IsProcessing &&
                    (r.NextRetryAt == null || r.NextRetryAt <= cutoffUtcTime))
            .OrderBy(r => r.CreatedAt)
            .Take(takeCount)
            .ToListAsync(cancellationToken);
    }
}