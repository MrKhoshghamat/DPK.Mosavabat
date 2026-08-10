using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface IAccountBalanceAverageDatabaseService : IMainDatabaseService
{
    Task<List<GetAccountBalanceAverageDto>?> GetAccountBalanceAverage(GetAccountBalanceAverageVm viewModel, CancellationToken cancellationToken);
    Task<List<GetAccountBalanceAverageDto>?> GetRelatedAccountBalanceAverage(GetRelatedAccountBalanceAverageVM viewModel, CancellationToken cancellationToken);
    Task<List<ProvidingApprovedAmountFromCreditsDto>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsVm viewModel, CancellationToken cancellationToken);
    Task<List<GetAccountBalanceAverageTarhJariZendegiDto>> GetAccountBalanceAverageTarhJariZendegi(GetAccountBalanceAverageTarhJariZendegiVm viewModel, CancellationToken cancellationToken);
    Task<List<GetSharedCustomerNumbersDto>> GetSharedCustomerNumbers(GetSharedCustomerNumbersVM viewModel, CancellationToken cancellationToken);
    Task<List<GetRawAccountBalanceAverageDto>> GetRawAccountBalanceAverage(GetRawAccountBalanceAverageVm viewModel, CancellationToken cancellationToken);
}