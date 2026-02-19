using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace DPK.Mosavabat.DomainEntities.LoanBill.Repositories;

public interface ILoanBillDataRepository
{
    Task AddRangeAsync(
        List<LoanBillData> loanBillDataList,
        CancellationToken cancellationToken);

    Task<List<LoanBillData>> GetProcessedLoanBillsAsync(
        List<string> loanIds,
        CancellationToken cancellationToken);

    Task<LoanBillData?> GetByLoanIdAsync(
        string loanId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        LoanBillData entity,
        CancellationToken cancellationToken = default);

    Task CreateAsync(
        LoanBillData entity,
        CancellationToken cancellationToken = default);

    Task<List<LoanBillData>> GetOldLoanBillsAsync(
        DateTime cutoffUtcTime,
        CancellationToken cancellationToken);

    Task DeleteRangeAsync(
        IEnumerable<LoanBillData> entities,
        CancellationToken cancellationToken);
}