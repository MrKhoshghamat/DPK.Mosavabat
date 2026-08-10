using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface IAccountBalanceAverageService
{
    Task<List<GetAccountBalanceAverageDto>?> GetAccountBalanceAverage(GetAccountBalanceAverageVm viewModel, CancellationToken cancellationToken = default);
    Task<List<GetAccountBalanceAverageDto>?> GetRelatedAccountBalanceAverage(GetRelatedAccountBalanceAverageVM viewModel, CancellationToken cancellationToken = default);
    Task<List<ProvidingApprovedAmountFromCreditsDto>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsVm viewModel, CancellationToken cancellationToken = default);
    Task<List<GetAccountBalanceAverageTarhJariZendegiDto>> GetAccountBalanceAverageTarhJariZendegi(GetAccountBalanceAverageTarhJariZendegiVm viewModel, CancellationToken cancellationToken = default);
    Task<List<GetAccountBalanceAverageDto>> GetSharedAccountBalanceAverage(GetSharedAccountBalanceAverageVM viewModel, CancellationToken cancellationToken = default);
    Task<List<GetRawAccountBalanceAverageDto>> GetRawAccountBalanceAverage(GetRawAccountBalanceAverageVm viewModel, CancellationToken cancellationToken);
}