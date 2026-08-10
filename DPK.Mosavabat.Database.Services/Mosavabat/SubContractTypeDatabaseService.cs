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

public class SubContractTypeDatabaseService : MainDatabaseService, ISubContractTypeDatabaseService
{
    private readonly ILoggerManager _logger;
    public SubContractTypeDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<ExtractSubContractTypeIdDto?> ExtractSubContractTypeId(ExtractSubContractTypeIdVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
            {new("@contractTemplateCode", SqlDbType.Int) {Value = viewModel.ContractTemplateCode}};

        var result =
            await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[LCP].[ExtractSubContractTypeId]",
                Parameters = parameters
            }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new ExtractSubContractTypeIdDto() { SubContractId = result.Result?.ToString() };
    }
}