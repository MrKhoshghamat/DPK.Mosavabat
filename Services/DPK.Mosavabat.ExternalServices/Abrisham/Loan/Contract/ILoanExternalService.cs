using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.LoanBills;

namespace DPK.Mosavabat.ExternalServices.Abrisham.Loan.Contract;

public interface ILoanExternalService : ITransientDependency
{
    Task<GetLoanBillDto> GetLoanBill(
        GetLoanBillDm dm,
        CancellationToken cancellationToken = default);
}