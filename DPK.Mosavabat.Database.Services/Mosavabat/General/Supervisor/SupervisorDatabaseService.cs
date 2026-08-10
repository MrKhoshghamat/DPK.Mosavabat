using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General.Supervisor;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class SupervisorDatabaseService : MainDatabaseService, ISupervisorDatabaseService
{
    private readonly ILoggerManager _logger;
    public SupervisorDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<FindRegionAndBrancheBySupervisorResponseModel?> FindRegionAndBrancheBySupervisor(FindRegionAndBrancheBySupervisorVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter> { new("@IdSupervisor", SqlDbType.Int) { Value = viewModel.IdSupervisor } };

        var result = await ExecuteAsListAsync<FindRegionAndBrancheBySupervisorResponseModel>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[MO].[FindRegionAndBrancheBySupervisor]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
        var row = result.List.FirstOrDefault();
        if(row != null)
        {
            return new FindRegionAndBrancheBySupervisorResponseModel() { IdBranche = row.IdBranche, IdRegion = row.IdRegion };
        }
        return default;
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
}