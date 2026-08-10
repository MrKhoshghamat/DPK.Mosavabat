using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General.Supervisor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class SupervisorController : ControllerBase
{
    private readonly ISupervisorService _supervisorService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public SupervisorController(ISupervisorService supervisorService, ILoggerManager logger, IMapper mapper)
    {
        _supervisorService = supervisorService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// پیدا کردن کد ناحیه و شعبه با شناسه سوپروایزر
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Supervisor" })]
    public async Task<IActionResult> FindRegionAndBrancheBySupervisor(FindRegionAndBrancheBySupervisorDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var viewModel = _mapper.Map<FindRegionAndBrancheBySupervisorVm>(dataModel);
        var result = await _supervisorService.FindRegionAndBrancheBySupervisor(viewModel);

        var response = new AjaxResponseBase<FindRegionAndBrancheBySupervisorResponseModel?>
        {
            Result = result,
            Success = result != null
        };

        return Ok(response);
    }
}