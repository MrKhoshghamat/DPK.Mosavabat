using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Newtonsoft.Json;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;

namespace DPK.Mosavabat.External.Services.Lcp;

public class RequestFacilityServices : IRequestFacilityServices
{
    private readonly ILoggerManager _logger;
    private readonly IServiceProvider _serviceProvider;
    public RequestFacilityServices(ILoggerManager logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<string?> AcceptRequestFacility(UpdateRequestFacilityDataModel updateRequestFacilityDataModel)
    {
        var response = await _serviceProvider.AcceptRequestFacilityService(updateRequestFacilityDataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }

    public async Task<string?> UpdateRadNumberRequestFacility(
        UpdateRequestFacilityRadNumber updateRequestFacilityRadNumber)
    {
        var response = await _serviceProvider.UpdateRadNumberRequestFacilityService(updateRequestFacilityRadNumber);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }
}