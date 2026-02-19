using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetCustomerContractsPastDebts;

/// <summary>
/// هندلر سرویس دریافت بدهی گذشته مشتری
/// </summary>
internal class GetCustomerContractsPastDebtsQueryHandler (
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetCustomerContractsPastDebtsQuery,
        List<CustomerPastDebtDto>>
{
    public async Task<List<CustomerPastDebtDto>> Handle(
        GetCustomerContractsPastDebtsQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.GetCustomerContractsPastDebts(
            mapper.Map<GetCustomerContractsPastDebtsDm>(query),
            cancellationToken);
    }
}