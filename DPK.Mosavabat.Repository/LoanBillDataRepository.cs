using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Entities.Models.LoanBill;
using Microsoft.EntityFrameworkCore;

namespace DPK.Mosavabat.Repository;

public class LoanBillDataRepository : RepositoryBase<LoanBillData>, ILoanBillDataRepository
{
    private readonly RepositoryContext _repositoryContext;

    public LoanBillDataRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public async Task AddRangeAsync(List<LoanBillData> loanBillDataList, CancellationToken cancellationToken)
    {
        await _repositoryContext.LoanBillDatas.AddRangeAsync(loanBillDataList, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _repositoryContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<LoanBillData>> GetProcessedLoanBillsAsync(
        List<string> loanIds,
        CancellationToken cancellationToken)
    {
        return await _repositoryContext.LoanBillDatas
            .Where(x => loanIds.Contains(x.LoanId))
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(LoanBillData entity)
    {
        _repositoryContext.LoanBillDatas.Update(entity);
    }

    public async Task CreateAsync(LoanBillData entity)
    {
        await _repositoryContext.LoanBillDatas.AddAsync(entity);
    }

    public async Task<LoanBillData?> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken)
    {
        return await _repositoryContext.LoanBillDatas
            .FirstOrDefaultAsync(x => x.LoanId == loanId, cancellationToken);
    }
}