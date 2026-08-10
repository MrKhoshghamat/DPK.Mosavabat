using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class ApprovalRequestDatabase : MainDatabase, IApprovalRequestDatabase
{
    private readonly ILoggerManager _logger;
    public ApprovalRequestDatabase(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    // لیست وضعیت یک درخواست را بر می گرداند
    public async Task<List<RequestFacilityStatusLevelDataModel>?> GetRequestStatus(GetRequestStatuesDataModel getRequestStatuesDataModel)
    {
        var parameters = new List<SqlParameter>
        {
            new("@idCase", SqlDbType.Int) {Value = getRequestStatuesDataModel.IdCase}
        };

        var response = await ExecuteAsListAsync<RequestFacilityStatusLevelDataModel>(DevProviderConnectionString, "[LCP].[GetRequestStatus]", parameters);
        
        _logger.Trace(JsonConvert.SerializeObject(response));
        
        if (response == null) return response;

        foreach (var item in response)
        {
            item.StatusDescription = item?.StatusDescription;
        }
        return response;

    }
}