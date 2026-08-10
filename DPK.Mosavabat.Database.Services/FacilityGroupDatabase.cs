using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class FacilityGroupDatabase : MainDatabase, IFacilityGroupDatabase
{
    private readonly ILoggerManager _logger;
    public FacilityGroupDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }
    // todo.m.kh create datamodel for code
    // استخراج شناسه نوع عقد
    public async Task<string?> ExtractFacilityGroupId(string code)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
            {new("@code", SqlDbType.Int) {Value = code}};

        // شناسه نوع عقد
        var facilityGroupId =
            await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[ExtractFacilityGroupId]", parameters);

        _logger.Trace(JsonConvert.SerializeObject(facilityGroupId));
        // برگشت شناسه نوع عقد
        return facilityGroupId?.ToString();
    }
}