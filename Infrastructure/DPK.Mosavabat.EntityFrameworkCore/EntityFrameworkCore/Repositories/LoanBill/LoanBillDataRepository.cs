using DPK.Foundation.Domain.Repositories;
using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using DPK.Mosavabat.DomainEntities.LoanBill.Repositories;

namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore.Repositories.LoanBill;

public class LoanBillDataRepository(
    IRepository<LoanBillData> repository)
    : ILoanBillDataRepository
{
    public async Task AddRangeAsync(
        List<LoanBillData> loanBillDataList,
        CancellationToken cancellationToken)
    {
        foreach (var entity in loanBillDataList)
        {
            await repository
                .InsertAsync(entity);
        }
    }

    public async Task<List<LoanBillData>> GetProcessedLoanBillsAsync(
        List<string> loanIds,
        CancellationToken cancellationToken)
    {
        var query = await repository
            .GetAllReadonlyAsync();

        var result = query
            .Where(x => loanIds
                .Contains(x.LoanId));

        return await result
            .ToListAsync(cancellationToken);
    }

    public async Task<LoanBillData?> GetByLoanIdAsync(
        string loanId,
        CancellationToken cancellationToken)
    {
        var query = await repository
            .GetAllAsync();

        return await query
            .FirstOrDefaultAsync(x =>
                x.LoanId == loanId,
                cancellationToken);
    }

    public Task UpdateAsync(
        LoanBillData entity,
        CancellationToken cancellationToken = default)
    {
        return repository
            .UpdateAsync(entity);
    }

    public Task CreateAsync(
        LoanBillData entity,
        CancellationToken cancellationToken = default)
    {
        return repository
            .InsertAsync(entity);
    }

    public async Task<List<LoanBillData>> GetOldLoanBillsAsync(
        DateTime cutoffUtcTime,
        CancellationToken cancellationToken)
    {
        // استفاده از GetAllReadonlyAsync معادل AsNoTracking است
        var query = await repository
            .GetAllReadonlyAsync();

        return await query
            .Where(ld => ld.CreatedAt < cutoffUtcTime)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(
        IEnumerable<LoanBillData>? entities,
        CancellationToken cancellationToken)
    {
        if (entities == null || !entities.Any())
        {
            return;
        }

        foreach (var entity in entities)
        {
            await repository
                .DeleteAsync(entity);
        }
    }
}