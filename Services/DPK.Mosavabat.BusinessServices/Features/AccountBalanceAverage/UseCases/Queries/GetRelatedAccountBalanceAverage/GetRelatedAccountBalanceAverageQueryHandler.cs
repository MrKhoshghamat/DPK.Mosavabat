using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.AccountBalanceAverage;
using DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;
using DPK.Mosavabat.Models.Queries.AccountBalanceAverage;
using DPK.Mosavabat.SqlBusinessServices.AccountBalanceAverage.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.AccountBalanceAverage.UseCases.Queries.GetRelatedAccountBalanceAverage;

/// <summary>
/// هندلر سرویس دریافت معدل موجودی حساب وابسته
/// </summary>
internal class GetRelatedAccountBalanceAverageQueryHandler(
    IAccountBalanceAverageSqlService accountBalanceAverageSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetRelatedAccountBalanceAverageQuery,
        List<AccountBalanceAverageDto>>
{
    public async Task<List<AccountBalanceAverageDto>> Handle(
        GetRelatedAccountBalanceAverageQuery query,
        CancellationToken cancellationToken)
    {
        return await accountBalanceAverageSqlService.GetRelatedAccountBalanceAverage(
            mapper.Map<GetRelatedAccountBalanceAverageDm>(query),
            cancellationToken);
    }
}