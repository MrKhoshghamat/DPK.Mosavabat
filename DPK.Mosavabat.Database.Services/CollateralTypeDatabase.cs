using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class CollateralTypeDatabase : MainDatabase, ICollateralTypeDatabase
{
    private readonly ILoggerManager _logger;
    public CollateralTypeDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // شناسه نوع وثیقه را بر می گرداند
    public async Task<string?> GetId(int? code = 0)
    {
        // پارامترها
        var parameters = new List<SqlParameter> { new("@Code", SqlDbType.Int) {Value = code} };

        // جستجوی کاربر
        var collateralTypeId = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[GetCollateralTypeId]", parameters);

        _logger.Trace(JsonConvert.SerializeObject(collateralTypeId));

        // برگشت نام کاربری
        return collateralTypeId?.ToString();
    }
}