using System.Data;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class AccountBalanceAverageDatabaseService : MainDatabaseService, IAccountBalanceAverageDatabaseService
{
    private readonly ILoggerManager _logger;

    public AccountBalanceAverageDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<List<GetAccountBalanceAverageDto>?> GetAccountBalanceAverage(GetAccountBalanceAverageVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@customerNo", SqlDbType.NVarChar) {Value = viewModel.CustomerNo},
            new("@approvalRequestId", SqlDbType.Int) {Value = viewModel.ApprovalRequestId},
            new("@cuMosavabeId", SqlDbType.Int) {Value = viewModel.CuMosavabeId},
            new("@typeOfContractLength", SqlDbType.Int) {Value = viewModel.TypeOfContractLength},
            new("@maxContractLength", SqlDbType.Int) {Value = viewModel.MaxContractLength},
            new("@code", SqlDbType.NVarChar) {Value = viewModel.Code},
            new("@currentMonth", SqlDbType.NVarChar) {Value = viewModel.CurrentMonth},
            new("@facilityGroup", SqlDbType.Int) {Value = viewModel.FacilityGroup},
        };

        var result = await ExecuteAsListAsync<GetAccountBalanceAverageDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[aba].[GetAccountBalanceAverage]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return result.List;
    }


    public async Task<List<GetAccountBalanceAverageDto>?> GetRelatedAccountBalanceAverage(GetRelatedAccountBalanceAverageVM viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@customerNo", SqlDbType.NVarChar) {Value = viewModel.CustomerNo},
            new("@accountNumber", SqlDbType.NVarChar) {Value = viewModel.AccountNumber},
            new("@approvalRequestId", SqlDbType.Int) {Value = viewModel.ApprovalRequestId},
            new("@cuMosavabeId", SqlDbType.Int) {Value = viewModel.CuMosavabeId},
            new("@typeOfContractLength", SqlDbType.Int) {Value = viewModel.TypeOfContractLength},
            new("@maxContractLength", SqlDbType.Int) {Value = viewModel.MaxContractLength},
            new("@code", SqlDbType.NVarChar) {Value = viewModel.Code},
            new("@currentMonth", SqlDbType.NVarChar) {Value = viewModel.CurrentMonth},
            new("@facilityGroup", SqlDbType.Int) {Value = viewModel.FacilityGroup},
        };

        var result = await ExecuteAsListAsync<GetAccountBalanceAverageDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[aba].[GetRelatedAccountBalanceAverage]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return result.List;
    }


    public async Task<List<ProvidingApprovedAmountFromCreditsDto>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsVm viewModel,
        CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@customerNo", SqlDbType.NVarChar) {Value = viewModel.CustomerNo},
            new("@approvedAmount", SqlDbType.Int) {Value = viewModel.ApprovedAmount},
            new("@approvalRequestId", SqlDbType.Int) {Value = viewModel.ApprovalRequestId},
            new("@cuMosavabatId", SqlDbType.Int) {Value = viewModel.CuMosavabatId},
        };

        var result = await ExecuteAsListAsync<ProvidingApprovedAmountFromCreditsDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[aba].[Test_PAAFC_V2]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
        return result.List;
    }

    public async Task<List<GetAccountBalanceAverageTarhJariZendegiDto>> GetAccountBalanceAverageTarhJariZendegi(GetAccountBalanceAverageTarhJariZendegiVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CustomerNumber", SqlDbType.VarChar) {Value = viewModel.CustomerNumber},
            new("@FacilityGroup", SqlDbType.Int) {Value = viewModel.FacilityGroup},
            new("@FacilitySubTypeCode", SqlDbType.Int) {Value = viewModel.FacilitySubTypeCode},
            new("@FromDate", SqlDbType.VarChar) {Value = viewModel.FromDate},
            new("@PeriodTime", SqlDbType.Int) {Value = viewModel.PeriodTime}
            
        };

        var result = await ExecuteAsListAsync<GetAccountBalanceAverageTarhJariZendegiDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[aba].[GetAccountBalanceAverageTarhJariZendegi]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return result.List;
    }

    public async Task<List<GetSharedCustomerNumbersDto>> GetSharedCustomerNumbers(GetSharedCustomerNumbersVM viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CustomerNumber",SqlDbType.VarChar) {Value = viewModel.CustomerNumber}
        };

        var result = await ExecuteAsListAsync<GetSharedCustomerNumbersDto>(new ExecuteAsListAsyncVm() 
        {
            ConnectionString= DevProviderConnectionString,
            StoredProcedureName = "[aba].[GetSharedCustomerNumbersByCustomerNo]",
            Parameters=parameters
        },cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
        return result.List;
    }

    public async Task<List<GetRawAccountBalanceAverageDto>> GetRawAccountBalanceAverage(GetRawAccountBalanceAverageVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@customerNo", SqlDbType.NVarChar) {Value = viewModel.CustomerNo},
            new("@currentMonth", SqlDbType.NVarChar) {Value = viewModel.CurrentMonth}

        };

        var result = await ExecuteAsListAsync<GetRawAccountBalanceAverageDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[aba].[GetRawAccountBalanceAverages]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return result.List;
    }
}