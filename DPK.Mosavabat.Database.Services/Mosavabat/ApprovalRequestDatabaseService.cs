using System.Data;
using System.Xml.Linq;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class ApprovalRequestDatabaseService : MainDatabaseService, IApprovalRequestDatabaseService
{
    private readonly ILoggerManager _logger;
    public ApprovalRequestDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<List<GetRequestLastStatusDto>> GetRequestsLastStatusBulk(List<int> caseIds,
        CancellationToken cancellationToken = default)
    {
        if (caseIds == null || !caseIds.Any())
            return new List<GetRequestLastStatusDto>();

        if (caseIds.Count > 100)
            throw new ArgumentException("Maximum 100 case IDs allowed.");

        var xml = new XElement("Ids", caseIds.Distinct().Select(id => new XElement("Id", id)));
        var xmlString = xml.ToString();

        var parameters = new List<SqlParameter>
        {
            new ("@caseIdsXml", SqlDbType.Xml) { Value = xmlString }
        };

        var result = await ExecuteAsListAsync<GetRequestLastStatusDto>(
            new ExecuteAsListAsyncVm
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[LCP].[GetRequestsLastStatus]",
                Parameters = parameters
            },
            cancellationToken);

        var resultList = result?.List ?? new List<GetRequestLastStatusDto>();

        _logger.Trace($"Bulk fetch returned {resultList.Count} records.");

        return resultList;
    }

    public async Task<List<GetRequestStatusDto>?> GetRequestStatus(GetRequestStatusVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@idCase", SqlDbType.Int) {Value = viewModel.IdCase}
        };

        var result = await ExecuteAsListAsync<GetRequestStatusDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetRequestStatus]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        foreach (var item in result.List)
        {
            item.StatusDescription = item?.StatusDescription;
        }

        return result.List;
    }

    public async Task<IsExistFacilitySubTypeCodeInGroupCreditPackageDto> IsExistFacilitySubTypeCodeInGroupCreditPackage(IsExistFacilitySubTypeCodeInGroupCreditPackageVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
            {
                new("@FacilitySubTypeCode", SqlDbType.VarChar) {Value = viewModel.FacilitySubTypeCode}
            };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[IsExistFacilitySubTypeCodeInGroupCreditPackage]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new IsExistFacilitySubTypeCodeInGroupCreditPackageDto() { IsExist = Convert.ToBoolean(result!.Result!) };
    }
}