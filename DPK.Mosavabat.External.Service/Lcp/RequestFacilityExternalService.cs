using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using Newtonsoft.Json;

namespace DPK.Mosavabat.External.Services.Lcp;

public class RequestFacilityExternalService : IRequestFacilityExternalService
{
    private readonly IExternalServiceProvider _externalServiceProvider;
    private readonly ILoggerManager _logger;

    public RequestFacilityExternalService(
        IExternalServiceProvider externalServiceProvider, 
        ILoggerManager logger)
    {
        _externalServiceProvider = externalServiceProvider;
        _logger = logger;
    }

    public async Task<UpdateRequestFacilityDto?> AcceptRequestFacility(UpdateRequestFacilityVm viewModel, CancellationToken cancellationToken)
    {
        var response = await _externalServiceProvider.AcceptRequestFacilityService(viewModel, cancellationToken);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }

    public async Task<UpdateRequestFacilityRadNumberDto?> UpdateRadNumberRequestFacility(UpdateRequestFacilityRadNumberVm viewModel, CancellationToken cancellationToken)
    {
        var response = await _externalServiceProvider.UpdateRadNumberRequestFacilityService(viewModel, cancellationToken);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }
}