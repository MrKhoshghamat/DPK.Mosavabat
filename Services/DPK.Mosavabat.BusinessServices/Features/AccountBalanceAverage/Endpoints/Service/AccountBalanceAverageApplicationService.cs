using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.AccountBalanceAverage.Endpoints.Contract;
using DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;
using DPK.Mosavabat.Models.Queries.AccountBalanceAverage;
using DPK.Mosavabat.Models.ViewModels.AccountBalanceAverage;

namespace DPK.Mosavabat.BusinessServices.Features.AccountBalanceAverage.Endpoints.Service;

/// <summary>
/// سرویس های معدل موجودی
/// </summary>
/// <param name="executionModule"></param>
public class AccountBalanceAverageApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IAccountBalanceAverageApplicationService
{
    /// <summary>
    /// سرویس دریافت معدل موجودی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<AccountBalanceAverageDto>> GetAccountBalanceAverage(
        GetAccountBalanceAverageVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetAccountBalanceAverageQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت معدل موجودی حساب وابسته
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<AccountBalanceAverageDto>> GetRelatedAccountBalanceAverage(
        GetRelatedAccountBalanceAverageVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetRelatedAccountBalanceAverageQuery>(vm),
            cancellationToken);
    }
}