using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Presentation.ActionFilters;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
//[Authorize]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly ILoggerManager _logger;
    public TokenController(IServiceManager service, ILoggerManager logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// صدور توکن جدید
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Authentication" })]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RefreshToken([FromBody] RepairAccessTokenDto dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(dataModel);

        var response = new AjaxResponseBase<TokenDto?>
        {
            Result = tokenDtoToReturn,
            Success = true
        };

        return Ok(response);
    }
}