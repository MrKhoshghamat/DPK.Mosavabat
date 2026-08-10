using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class BizagiController : ControllerBase
{
    private readonly IBizagiService _bizagiService;
    private readonly ILoggerManager _logger;

    public BizagiController(IBizagiService bizagiService, ILoggerManager logger)
    {
        _bizagiService = bizagiService;
        _logger = logger;
    }

    /// <summary>
    /// برگشت تمام کیس ها برا ابطال
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Bizagi" })]
    public async Task<IActionResult> ExtractAllCasesForAbortCase()
    {
        var result = await _bizagiService.ExtractAllCasesForAbortCase();
        return Ok(CreateResponse(result));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Bizagi" })]
    public async Task<IActionResult> IsWfUser(IsWfUserVm vm)
    {
        var result = await _bizagiService.IsWfUser(vm);
        return Ok(CreateResponse(result));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Bizagi" })]
    public async Task<IActionResult> HasSupportRole(HasSupportRoleVm vm)
    {
        var result = await _bizagiService.HasSupportRole(vm);
        return Ok(CreateResponse(result));
    }

    /// <summary>
    /// ساخت پاسخ
    /// </summary>
    protected AjaxResponseBase<T> CreateResponse<T>(T? result = default, ErrorInfo? error = null)
    {
        // Creating a new response object
        var response = new AjaxResponseBase<T>
        {
            Result = result, // Assigning result to response
            Success = error == null, // Setting success based on error presence
            Error = error // Assigning error to response
        };
        // Logging the response
        _logger.Trace(JsonConvert.SerializeObject(response));
        // Returning the response
        return response;
    }
}