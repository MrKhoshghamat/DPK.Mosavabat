using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class SubContractTypeDatabase : MainDatabase, ISubContractTypeDatabase
{
    private readonly ILoggerManager _logger;
    public SubContractTypeDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }
    // todo.m.kh create datamodel for contractTemplateCode
    // استخراج نوع قرارداد
    public async Task<string?> ExtractSubContractTypeId(string contractTemplateCode)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
            {new("@contractTemplateCode", SqlDbType.Int) {Value = contractTemplateCode}};

        // شناسه نوع قرارداد
        var subContractTypeId =
            await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[ExtractSubContractTypeId]", parameters);

        _logger.Trace(JsonConvert.SerializeObject(subContractTypeId));
        // برگشت شناسه نوع قرارداد
        return subContractTypeId?.ToString();
    }
}