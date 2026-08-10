using System.Data;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class FacilityGroupDatabaseService : MainDatabaseService, IFacilityGroupDatabaseService
{
    private readonly ILoggerManager _logger;
    public FacilityGroupDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<ExtractFacilityGroupIdDto?> ExtractFacilityGroupId(ExtractFacilityGroupIdVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
            {new("@code", SqlDbType.Int) {Value = viewModel.Code}};

        var result =
            await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[LCP].[ExtractFacilityGroupId]",
                Parameters = parameters
            }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new ExtractFacilityGroupIdDto() { FacilityGroupId = result.Result?.ToString() };
    }
}