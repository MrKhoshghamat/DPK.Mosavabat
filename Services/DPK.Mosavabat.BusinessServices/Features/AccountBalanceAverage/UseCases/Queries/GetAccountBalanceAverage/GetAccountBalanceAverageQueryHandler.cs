using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.AccountBalanceAverage;
using DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;
using DPK.Mosavabat.Models.Queries.AccountBalanceAverage;
using DPK.Mosavabat.SqlBusinessServices.AccountBalanceAverage.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.AccountBalanceAverage.UseCases.Queries.GetAccountBalanceAverage;

/// <summary>
/// هندلر سرویس دریافت معدل موجودی
/// </summary>
internal class GetAccountBalanceAverageQueryHandler(
    IAccountBalanceAverageSqlService accountBalanceAverageSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetAccountBalanceAverageQuery,
        List<AccountBalanceAverageDto>>
{
    public async Task<List<AccountBalanceAverageDto>> Handle(
        GetAccountBalanceAverageQuery query,
        CancellationToken cancellationToken)
    {
        return await accountBalanceAverageSqlService.GetAccountBalanceAverage(
            mapper.Map<GetAccountBalanceAverageDm>(query),
            cancellationToken);
    }
}