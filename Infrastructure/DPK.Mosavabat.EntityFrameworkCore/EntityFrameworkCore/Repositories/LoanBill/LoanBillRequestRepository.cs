using DPK.Foundation.Domain.Repositories;
using DPK.Mosavabat.Common.Helpers;
using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using DPK.Mosavabat.DomainEntities.LoanBill.Repositories;

namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore.Repositories.LoanBill;

public class LoanBillRequestRepository(
    IRepository<LoanBillRequest> repository)
    : ILoanBillRequestRepository
{

    public async Task AddRangeAsync(
        List<LoanBillRequest> requests,
        CancellationToken cancellationToken)
    {
        foreach (var request in requests)
        {
            await repository.InsertAsync(request);
        }
    }

    public Task UpdateAsync(
        LoanBillRequest request,
        CancellationToken cancellationToken)
    {
        return repository.UpdateAsync(request);
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

        var query = await repository
            .GetAllReadonlyAsync();

        var pendingRequests = query
            .Where(r =>
                !r.IsCompleted &&
                !r.IsProcessing &&
                (r.NextRetryAt == null || r.NextRetryAt <= cutoffUtcTime))
            .OrderBy(r => r.CreatedAt)
            .Take(takeCount);

        return await pendingRequests
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LoanBillRequest>> GetActiveRequestsByCustomerAsync(
        string customerNo,
        DateTime cutoffUtcTime,
        CancellationToken cancellationToken)
    {
        var query = await repository
            .GetAllReadonlyAsync();

        return await query
            .Where(r =>
                r.CustomerNo == customerNo &&
                !r.IsCompleted &&
                r.CreatedAt >= cutoffUtcTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> GetIsProcessingRequestsAsync(
        string customerNo,
        DateTime cutoffUtcTime,
        CancellationToken cancellationToken)
    {
        var query = await repository
            .GetAllReadonlyAsync();

        return await query.AnyAsync(
            r =>
                r.CustomerNo == customerNo &&
                !r.IsCompleted &&
                r.CreatedAt >= cutoffUtcTime,
            cancellationToken);
    }
}