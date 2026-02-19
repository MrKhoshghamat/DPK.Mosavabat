using DPK.Foundation.Dependency;
using Dpk.UseAdanicAbrishamService.Services;

namespace DPK.Mosavabat.ExternalServices.Abrisham.Loan.Proxy;

public interface ILoanProxyService
    : ITransientDependency
{
    Task<LoanBillDto> GetLoanBillAsync(LoanBillVM vm,
        CancellationToken cancellationToken);

    Task<LoanBillV2Dto> GetLoanBillV2Async(LoanBillV2VM vm,
        CancellationToken cancellationToken);
}