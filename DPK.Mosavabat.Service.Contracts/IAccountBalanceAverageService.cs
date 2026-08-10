using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.ResponseModels;

namespace DPK.Mosavabat.Services.Contracts
{
    public interface IAccountBalanceAverageService
    {
        Task<List<GetAccountBalanceAverageResponseModel>?> GetAccountBalanceAverage(AccountBalanceAverageDataModel accountBalanceAverageDataModel);
        Task<List<ProvidingApprovedAmountFromCreditsResponseModel>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsDataModel providingApprovedAmountFromCreditsDataModel);
    }
}
