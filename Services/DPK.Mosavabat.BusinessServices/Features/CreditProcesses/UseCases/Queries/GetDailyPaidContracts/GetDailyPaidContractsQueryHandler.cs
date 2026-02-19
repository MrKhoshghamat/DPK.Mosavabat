using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetDailyPaidContracts;

/// <summary>
/// هندلر سرویس دریافت قراردادهای پرداخت شده به مشتری در روز
/// </summary>
internal class GetDailyPaidContractsQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetDailyPaidContractsQuery,
        List<CustomerDailyPaidContractDto>>
{
    public async Task<List<CustomerDailyPaidContractDto>> Handle(
        GetDailyPaidContractsQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.GetCustomerDailyPaidContracts(
            mapper.Map<GetCustomerDailyPaidContractsDm>(query),
            cancellationToken);
    }
}