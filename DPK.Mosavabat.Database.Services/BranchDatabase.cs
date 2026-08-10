using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class BranchDatabase : MainDatabase, IBranchDatabase
{
    private readonly ILoggerManager _logger;
    public BranchDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // استخراج کد شعبه
    public async Task<string?> ExtractBranchCode(int idLcpRequestFacility)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
            {new("@idLcpRequestFacility", SqlDbType.Int) {Value = idLcpRequestFacility}};

        // کد شعبه
        var branchCode =
            await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[ExtractBranchCode]", parameters);

        _logger.Trace(JsonConvert.SerializeObject(branchCode));
        // برگشت کد شعبه
        return branchCode?.ToString();

    }
}