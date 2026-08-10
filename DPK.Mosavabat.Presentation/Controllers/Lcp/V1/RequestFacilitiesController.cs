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
public class RequestFacilitiesController : ControllerBase
{
    private readonly IRequestFacilityServices _requestFacilityServices;
    private readonly ILoggerManager _logger;

    public RequestFacilitiesController(IRequestFacilityServices requestFacilityServices, ILoggerManager logger)
    {
        _requestFacilityServices = requestFacilityServices;
        _logger = logger;
    }

    /// <summary>
    /// تایید درخواست تسهیلات اولیه
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<object?> AcceptRequestFacility(UpdateRequestFacilityDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _requestFacilityServices.AcceptRequestFacility(dataModel);
    }

    /// <summary>
    /// به روز رسانی شماره پرونده درخواست تسهیلات اولیه
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<string?> UpdateRequestFacilityRadNumber(UpdateRequestFacilityRadNumber dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _requestFacilityServices.UpdateRadNumberRequestFacility(dataModel);
    }
}