using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class CollateralGroupDatabase : MainDatabase, ICollateralGroupDatabase
{
    private readonly ILoggerManager _logger;
    public CollateralGroupDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // شناسه گروه وثیقه را بر می گرداند
    public async Task<string?> GetId(int code)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
            {new("@Code", SqlDbType.Int) {Value = code}};

        // فراخوانی روال و دریافت شناسه
        var collateralGroupId =
            await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[GetCollateralGroupId]", parameters);


        _logger.Trace(JsonConvert.SerializeObject(collateralGroupId));
        // برگشت کد شعبه
        return collateralGroupId?.ToString();
    }
}