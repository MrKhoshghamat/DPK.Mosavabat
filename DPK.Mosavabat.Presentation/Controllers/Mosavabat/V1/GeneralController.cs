using AutoMapper;
using DPK.Mosavabat.Common.Consts;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class GeneralController : ControllerBase
{
    //private readonly IServiceManager _service;
    private readonly IRequestFacilityService _requestFacilityService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public GeneralController(IRequestFacilityService requestFacilityService, ILoggerManager logger, IMapper mapper)
    {
        _requestFacilityService = requestFacilityService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// کنسل کردن مورد
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "General" })]
    public async Task<IActionResult> AbortCases(AbortCasesDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        // مواردی را که می خواهیم ابطال کنیم

        var viewModel = _mapper.Map<AbortCasesVm>(dataModel);
        var result = await _requestFacilityService.AbortCasesAsync(viewModel);

        var response = new AjaxResponseBase<MosavabatResponseDataModel?>
        {
            Result = result.MosavabatResponseDataModel,
            Success = result != null
        };

        return Ok(response);
    }

    /// <summary>
    /// گرفتن ورژن نرم افزار
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "General" })]
    public IActionResult CheckVersion()
    {
        return Ok($"Current MA Version: {CreditProcessesConst.Version}");
    }
}