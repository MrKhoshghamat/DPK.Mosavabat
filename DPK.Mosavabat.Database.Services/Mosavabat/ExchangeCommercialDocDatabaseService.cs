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

public class ExchangeCommercialDocDatabaseService : MainDatabaseService, IExchangeCommercialDocDatabaseService
{
    private readonly ILoggerManager _logger;
    public ExchangeCommercialDocDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<ExtractExchangeCommercialDocIdDto?> ExtractExchangeCommercialDocId(ExtractExchangeCommercialDocIdVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
            {new("@DocumentCode", SqlDbType.Int) {Value = Convert.ToInt32(viewModel.DocumentCode)}};

        var result =
            await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[LCP].[ExtractExchangeCommercialDocId]",
                Parameters = parameters
            }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
        // برگشت شناسه سند
        return new ExtractExchangeCommercialDocIdDto() { DocumentId = result?.Result.ToString() };
    }
}