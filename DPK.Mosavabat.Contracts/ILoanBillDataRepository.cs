using DPK.Mosavabat.Entities.Models.LoanBill;

namespace DPK.Mosavabat.Contracts;

public interface ILoanBillDataRepository : IRepositoryBase<LoanBillData>
{
    Task AddRangeAsync(List<LoanBillData> loanBillDataList, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<List<LoanBillData>> GetProcessedLoanBillsAsync(List<string> loanIds, CancellationToken cancellationToken);
    Task<LoanBillData?> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken);
    Task UpdateAsync(LoanBillData entity);
    Task CreateAsync(LoanBillData entity);
}