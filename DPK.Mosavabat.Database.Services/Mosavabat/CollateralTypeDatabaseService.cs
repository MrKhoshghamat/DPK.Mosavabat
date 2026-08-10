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

public class CollateralTypeDatabaseService : MainDatabaseService, ICollateralTypeDatabaseService
{
    private readonly ILoggerManager _logger;
    public CollateralTypeDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<GetCollateralTypeIdDto?> GetCollateralTypeId(GetCollateralTypeIdVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter> { new("@Code", SqlDbType.Int) { Value = viewModel.Code } };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetCollateralTypeId]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetCollateralTypeIdDto() { CollateralTypeId = result.Result?.ToString() };
    }
}