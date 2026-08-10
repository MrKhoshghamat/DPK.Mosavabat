using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class RequestFacilityDatabase : MainDatabase, IRequestFacilityDatabase
{
    private readonly ILoggerManager _logger;
    public RequestFacilityDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // به روز رسانی درخواست تسهیلات اولیه
    public async Task<string?> UpdateLcpRequestFacility(int idCase, int initialRadNumber, int? idCaseApprovalRequest, int? radNumberApprovalRequest,int? lcpRequestFacilityId, string? guidLcpRequestFacility)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
        {
            new("@idCase", SqlDbType.Int) {Value = idCase},
            new("@initialRadNumber", SqlDbType.Int) {Value = initialRadNumber},
            new("@idCaseApprovalRequest", SqlDbType.Int) {Value = idCaseApprovalRequest},
            new("@radNumberApprovalRequest", SqlDbType.Int) {Value = radNumberApprovalRequest},
            new("@lcpRequestFacilityId", SqlDbType.Int) {Value = lcpRequestFacilityId},
            new("@guidLcpRequestFacility", SqlDbType.NVarChar) {Value = guidLcpRequestFacility}
        };

        var rowsAffected = await ExecuteNonQueryAsync(DevProviderConnectionString,"[LCP].[UpdateLcpRequestFacility]",parameters);
        _logger.Trace(JsonConvert.SerializeObject(rowsAffected));
        return rowsAffected.ToString();
    }

    // نگاشت اطلاعات درخواست اولیه با درخواست اصلی
    public async Task<string?> MapLcpRequestFacilityToApprovalRequest(int idCase, int idCaseApprovalRequest, int idLcpRequestFacility)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
        {
            new("@idLCP_RequestFacility", SqlDbType.Int) {Value = idLcpRequestFacility},
            new("@idCase", SqlDbType.Int) {Value = idCase},
            new("@idCaseApprovalRequest", SqlDbType.Int) {Value = idCaseApprovalRequest}
        };

        var rowsAffected = await ExecuteNonQueryAsync(DevProviderConnectionString,
            "[LCP].[MapLcpRequestFacilityToApprovalRequest]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(rowsAffected));
        return rowsAffected.ToString();
    }

    // شناسه مورد درخواست تسهیلات اولیه را بر می گرداند
    public async Task<string?> GetRequestFacilityCaseId(int idCaseApprovalRequest)
    {
        // پارامترها
        var parameters = new List<SqlParameter>
        {
            new("@idCaseApprovalRequest", SqlDbType.Int) {Value = idCaseApprovalRequest}
        };

        var caseId = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[GetRequestFacilityCaseId]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(caseId));
        return caseId?.ToString();
    }

    // لینک فایل قرارداد را بر می گرداند
    public async Task<string?> GetContractFileLink(GetMorabeheContractFileLinkDataModel getMorabeheContractFileLinkDataModel)
    {
        var parameters = new List<SqlParameter>
        {
            new("@contractId", SqlDbType.Int) {Value = getMorabeheContractFileLinkDataModel.ContractId}
        };

        // var fileLink = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[GetMorabeheContractFileLink]", parameters);
        var fileLink = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[GetContractFileLink]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(fileLink));
        return fileLink?.ToString();
    }

    public async Task<string?> UpdateContractFileLink(UpdateMorabeheContractFileLink updateMorabeheContractFileLink)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@FileLink", SqlDbType.VarChar) {Value = updateMorabeheContractFileLink.FileLink},
            new("@CuApprovalSubjectId", SqlDbType.VarChar) {Value = updateMorabeheContractFileLink.CuApprovalSubjectId}
        };

        var result = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[UpdateContractFileLink]", parameters);
        _logger.Trace(JsonConvert.SerializeObject(result));
        return result?.ToString();
    }

    public async Task<CustomerAccountNoDataModel> FindCustomerAccountNoDataModel(CustomerNoDataModel customerNoDataModel)
    {
        var parameters = new List<SqlParameter>
        {
            new("@AccountNo", SqlDbType.Int){Value = customerNoDataModel.AccountNo}
        };

        var response = await ExecuteScalarAsync(DevProviderConnectionString, "[LCP].[FindCustomerAccountInDevProvider]", parameters);

        _logger.Trace(JsonConvert.SerializeObject(response));

        return new CustomerAccountNoDataModel()
        {
            DevProviderCustomerCount = Convert.ToInt32(response)
        };
    }
}