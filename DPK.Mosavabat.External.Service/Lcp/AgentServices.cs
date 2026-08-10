using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Newtonsoft.Json;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;
namespace DPK.Mosavabat.External.Services.Lcp;

public class AgentServices : IAgentServices
{
    private readonly ILoggerManager _logger;
    private readonly IServiceProvider _serviceProvider;

    public AgentServices(ILoggerManager logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<string?> AcceptAgent(AcceptAgentDataModel acceptAgent)
    {
        var response = await _serviceProvider.AcceptAgentService(acceptAgent);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }
}