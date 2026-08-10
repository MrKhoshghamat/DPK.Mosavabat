using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Contracts;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services;

public class UserDatabase : MainDatabase, IUserDatabase
{
    private readonly IBranchDatabase _branchDatabase;
    private readonly ILoggerManager _logger;
    public UserDatabase(IConfiguration configuration, IBranchDatabase branchDatabase, ILoggerManager logger) : base(configuration)
    {
        _branchDatabase = branchDatabase;
        _logger = logger;
    }

    // پیدا کردن مسوول تسهیلات
    public async Task<string?> FindFacilityManagerOrSupervisor(int idLcpRequestFacility)
    {
        // استخراج کد شعبه
        var branchCode = await _branchDatabase.ExtractBranchCode(idLcpRequestFacility);

        var parameters = new List<SqlParameter>
        {
            new("@branchCode", SqlDbType.Int) {Value = branchCode}
        };

        // جستجوی کاربر
        var username = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[FindFacilityManagerOrSupervisor]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(username));
        // برگشت نام کاربری
        return username?.ToString();
    }

    // دریافت نام کاربری کارتابل مصوبات
    public async Task<string?> GetUserName(int branchCode, MosavabatClassName mosavabatClassName)
    {
        var parameters = new List<SqlParameter>
        {
            new("@branchCode", SqlDbType.Int) {Value = branchCode}
        };

        var username = await ExecuteScalarAsync(DevProviderConnectionString, mosavabatClassName == MosavabatClassName.AgentRegistry ? "[LCP].[FindBranchManagerOrSupervisor]" : "[LCP].[FindFacilityManagerOrSupervisor]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(username));
        return username?.ToString();
    }

    // دریافت شناسه کاربری کارتابل مصوبات
    public async Task<int> GetUserId(string userName)
    {
        var parameters = new List<SqlParameter>
        {
            new("@UserName", SqlDbType.VarChar) {Value = userName}
        };

        var userId = await ExecuteScalarAsync(DevProviderConnectionString, "[CP].[FindUserIdByUserName]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(userId));
        return Convert.ToInt32(userId);
    }
}