using AutoMapper;
using DPK.Mosavabat.ExternalServices.Abrisham.Loan.Contract;
using DPK.Mosavabat.ExternalServices.Abrisham.Loan.Proxy;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.LoanBills;
using Dpk.UseAdanicAbrishamService.Services;

namespace DPK.Mosavabat.ExternalServices.Abrisham.Loan.Service;

public class LoanExternalService(
    ILoanProxyService loanProxyService,
    IMapper mapper)
    : ILoanExternalService
{
    public async Task<GetLoanBillDto> GetLoanBill(
        GetLoanBillDm dm,
        CancellationToken cancellationToken = default)
    {
        var response = await loanProxyService.GetLoanBillAsync(
            mapper.Map<LoanBillVM>(dm),
            cancellationToken);

        return mapper.Map<GetLoanBillDto>(response);
    }
}