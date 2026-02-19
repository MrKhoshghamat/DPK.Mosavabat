using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.IsAllowedRecalculateAccountBalanceAverage;

/// <summary>
/// هندلر سرویس مجاز به محاسبه مجدد معدل موجودی
/// </summary>
internal class IsAllowedRecalculateAccountBalanceAverageQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        IsAllowedRecalculateAccountBalanceAverageQuery,
        IsAllowedRecalculateAccountBalanceAverageDto>
{
    public async Task<IsAllowedRecalculateAccountBalanceAverageDto> Handle(
        IsAllowedRecalculateAccountBalanceAverageQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.IsAllowedRecalculateAccountBalanceAverage(
            mapper.Map<IsAllowedRecalculateAccountBalanceAverageDm>(query),
            cancellationToken);
    }
}