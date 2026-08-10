using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class ExchangeCommercialDocDatabase : MainDatabase, IExchangeCommercialDocDatabase
{
    private readonly ILoggerManager _logger;
    public ExchangeCommercialDocDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // استخراج شناسه سند
    public async Task<string?> ExtractExchangeCommercialDocId(string documentCode)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
            {new("@DocumentCode", SqlDbType.Int) {Value = Convert.ToInt32(documentCode)}};

        // شناسه سند
        var Id =
            await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[ExtractExchangeCommercialDocId]", parameters);
 
        _logger.Trace(JsonConvert.SerializeObject(Id));
        // برگشت شناسه سند
        return Id?.ToString();
    }
}