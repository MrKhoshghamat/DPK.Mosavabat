using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.AccountBalanceAverage;
using DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;

namespace DPK.Mosavabat.SqlBusinessServices.AccountBalanceAverage.Contract;

public interface IAccountBalanceAverageSqlService
    : ITransientDependency
{
    Task<List<AccountBalanceAverageDto>> GetAccountBalanceAverage(
        GetAccountBalanceAverageDm dm,
        CancellationToken cancellationToken = default);

    Task<List<AccountBalanceAverageDto>> GetRelatedAccountBalanceAverage(
        GetRelatedAccountBalanceAverageDm dm,
        CancellationToken cancellationToken = default);
}