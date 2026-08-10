using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
namespace DPK.Mosavabat.Presentation.Controllers.Lcp.V1;

//[Authorize]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "in-lcp-v1")]
[Route("api/lcp/[controller]/[action]")]
[ApiController]
public class AgentsController : ControllerBase
{
    private readonly IAgentServices _agentServices;
    private readonly IServiceManager _service;
    private readonly ILoggerManager _logger;
    private readonly ILcpService _lcpService;
    public AgentsController(IAgentServices agentServices, IServiceManager service, ILoggerManager logger, ILcpService lcpService)
    {
        _agentServices = agentServices;
        _service = service;
        _logger = logger;
        _lcpService = lcpService;
    }

    /// <summary>
    /// تایید نماینده
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [SwaggerOperation(Tags = new[] { "Agents" })]
    public async Task<string?> AcceptAgent(AcceptAgentDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _agentServices.AcceptAgent(dataModel);
    }
}