using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly ILoggerManager _logger;

    public AuthenticationController(IServiceManager service, ILoggerManager logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// ثبت یک کاربر
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Authentication" })]
    public async Task<IActionResult> Register(UserForRegistrationDto dataModel)
    {
        var result = await _service.AuthenticationService.RegisterUser(dataModel);
        if (result.Succeeded) return StatusCode(201);
        foreach (var error in result.Errors) ModelState.TryAddModelError(error.Code, error.Description);

        return BadRequest(ModelState);
    }

    /// <summary>
    /// ورود کاربر
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Authentication" })]
    public async Task<IActionResult> Login(UserForAuthenticationDto dataModel)
    {
        if (!await _service.AuthenticationService.ValidateUser(dataModel)) return Unauthorized();
        var tokenDto = await _service.AuthenticationService.CreateToken(true);
        var response = new AjaxResponseBase<TokenDto?>
        {
            Result = tokenDto,
            Success = true
        };
        return Ok(response);
    }
}