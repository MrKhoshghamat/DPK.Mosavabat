using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
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
public class ContractController : ControllerBase
{
    private readonly IContractServices _contractContractServices;
    private readonly ILoggerManager _logger;

    public ContractController(IContractServices contractContractServices, ILoggerManager logger)
    {
        _contractContractServices = contractContractServices;
        _logger = logger;
    }

    /// <summary>
    /// ثبت قرارداد
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [SwaggerOperation(Tags = new[] { "Contracts" })]
    public async Task<string?> InsertContract(InsertContractDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _contractContractServices.PrintContract(dataModel);
    }
}