using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.ResponseModels;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services;

public class AccountBalanceAverageService : IAccountBalanceAverageService
{
    private readonly IAccountBalanceAverageDatabase _accountBalanceAverage;
    private readonly ILoggerManager _logger;
    public AccountBalanceAverageService(ILoggerManager logger, IAccountBalanceAverageDatabase accountBalanceAverage)
    {
        _logger = logger;
        _accountBalanceAverage = accountBalanceAverage;
    }

    public async Task<List<GetAccountBalanceAverageResponseModel>?> GetAccountBalanceAverage(AccountBalanceAverageDataModel accountBalanceAverageDataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(accountBalanceAverageDataModel));
        return await _accountBalanceAverage.GetAccountBalanceAverage(accountBalanceAverageDataModel);
    }

    public async Task<List<ProvidingApprovedAmountFromCreditsResponseModel>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsDataModel providingApprovedAmountFromCreditsDataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(providingApprovedAmountFromCreditsDataModel));
        return await _accountBalanceAverage.ProvidingApprovedAmountFromCredits(providingApprovedAmountFromCreditsDataModel);
    }
}