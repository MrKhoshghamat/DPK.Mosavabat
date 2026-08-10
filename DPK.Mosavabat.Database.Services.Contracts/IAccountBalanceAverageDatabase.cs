using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.ResponseModels;

namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IAccountBalanceAverageDatabase : IMainDatabase
{
    Task<List<GetAccountBalanceAverageResponseModel>?> GetAccountBalanceAverage(AccountBalanceAverageDataModel accountBalanceAverageDataModel);
    Task<List<ProvidingApprovedAmountFromCreditsResponseModel>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsDataModel providingApprovedAmountFromCreditsDataModel);
}