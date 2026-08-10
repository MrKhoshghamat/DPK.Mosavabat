using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using DPK.Mosavabat.Shared.ResponseModels;
using DPK.Mosavabat.Contracts;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services;

public class AccountBalanceAverageDatabase : MainDatabase, IAccountBalanceAverageDatabase
{
    private readonly ILoggerManager _logger;
    public AccountBalanceAverageDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // دریافت معدل موجودی
    public async Task<List<GetAccountBalanceAverageResponseModel>?> GetAccountBalanceAverage(AccountBalanceAverageDataModel accountBalanceAverageDataModel)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
        {
            new("@customerNo", SqlDbType.NVarChar) {Value = accountBalanceAverageDataModel.CustomerNo},
            new("@approvalRequestId", SqlDbType.Int) {Value = accountBalanceAverageDataModel.ApprovalRequestId},
            new("@cuMosavabeId", SqlDbType.Int) {Value = accountBalanceAverageDataModel.CuMosavabeId},
            new("@typeOfContractLength", SqlDbType.Int) {Value = accountBalanceAverageDataModel.TypeOfContractLength},
            new("@maxContractLength", SqlDbType.Int) {Value = accountBalanceAverageDataModel.MaxContractLength},
            new("@code", SqlDbType.NVarChar) {Value = accountBalanceAverageDataModel.Code},
            new("@currentMonth", SqlDbType.NVarChar) {Value = accountBalanceAverageDataModel.CurrentMonth},
            new("@facilityGroup", SqlDbType.Int) {Value = accountBalanceAverageDataModel.FacilityGroup},
        };
        var result = await ExecuteAsListAsync<GetAccountBalanceAverageResponseModel>(DevProviderConnectionString, "[aba].[Test_CC_V2]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(result));
        return result;
    }

    // مصرف اعتبار
    public async Task<List<ProvidingApprovedAmountFromCreditsResponseModel>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsDataModel providingApprovedAmountFromCreditsDataModel)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
        {
            new("@customerNo", SqlDbType.NVarChar) {Value = providingApprovedAmountFromCreditsDataModel.CustomerNo},
            new("@approvedAmount", SqlDbType.Int) {Value = providingApprovedAmountFromCreditsDataModel.ApprovedAmount},
            new("@approvalRequestId", SqlDbType.Int) {Value = providingApprovedAmountFromCreditsDataModel.ApprovalRequestId},
            new("@cuMosavabatId", SqlDbType.Int) {Value = providingApprovedAmountFromCreditsDataModel.CuMosavabatId},
        };

        var result = await ExecuteAsListAsync<ProvidingApprovedAmountFromCreditsResponseModel>(DevProviderConnectionString, "[aba].[Test_PAAFC_V2]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(result));
        return result;
    }
}