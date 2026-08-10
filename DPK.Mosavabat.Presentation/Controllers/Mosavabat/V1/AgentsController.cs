using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class AgentsController : ControllerBase
{
    private readonly IAgentService _agentService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public AgentsController(IAgentService agentService, ILoggerManager logger, IMapper mapper)
    {
        _agentService = agentService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// ثبت نماینده
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "LegalCustomerPortalAdmin")]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Agents" })]
    [HttpPost]
    public async Task<IActionResult> Create(
        MosavabatAgentDataModel dataModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var checkExistenceModel = new CheckAgentRegistryExistsDm()
        {
            Agent = new AgentDataModel()
            {
                BranchCode = dataModel.BranchCode,
                NationalCode = dataModel.NationalCode,
                LegalNationalId = dataModel.LegalNationalId
            }
        };

        var existenceResult = await _agentService.CheckAgentRegistryExists(checkExistenceModel, cancellationToken);

        if (existenceResult.Exists)
        {
            var duplicateResponse = new AjaxResponseBase<MosavabatResponseDataModel?>
            {
                Success = false,
                Error = new ErrorInfo()
                {
                    Code = 409,
                    Message = "این نماینده پیش از این با همین شعبه و مشخصات حقوقی در سیستم ثبت شده است و امکان ثبت مجدد وجود ندارد."
                }
            };
            return Ok(duplicateResponse);
        }

        var viewModel = _mapper.Map<RegisterAgentVm>(dataModel);
        var result = await _agentService.RegisterAgentAsync(viewModel, cancellationToken);

        var response = new AjaxResponseBase<MosavabatResponseDataModel?>
        {
            Result = result?.MosavabatResponseDataModel,
            Success = true
        };

        var isSucceedUpdateCaseIdAndRadNumberInAgentRegistry = await _agentService.UpdateCaseIdAndRadNumberInAgentRegistry(
            new UpdateCaseIdAndRadNumberInAgentRegistryVm()
            {
                Agent = new AgentDataModel()
                {
                    BranchCode = dataModel.BranchCode,
                    LegalNationalId = dataModel.LegalNationalId,
                    NationalCode = dataModel.NationalCode
                },
                CaseId = response.Result?.CaseId ?? 0,
                RadNumber = response.Result?.RadNumber ?? 0
            }, cancellationToken);

        if (isSucceedUpdateCaseIdAndRadNumberInAgentRegistry.IsSucceed)
        {
            return Ok(response);
        }

        response.Success = false;
        response.Error = new ErrorInfo()
        {
            Code = 0,
            Message = "ثبت اولیه انجام شد، اما در بروزرسانی شناسه فرآیند و شماره درخواست ثبت نماینده مشکلی بوجود آمده است."
        };

        return Ok(response);
    }


    [HttpPost]
    //[Authorize(Roles = "LegalCustomerPortalAdmin")]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Agents" })]
    public async Task<IActionResult> RemoveAgentFromCartable(
        [FromBody] RemoveAgentFromCartableVm viewModel,
        CancellationToken cancellationToken = default)
    {
        if (viewModel is not { CaseId: > 0 })
            return BadRequest("caseId نامعتبر است");

        var getUserNameByCaseIdDto = await _agentService.GetUserNameByCaseId(new GetUserNameByCaseIdVm()
        {
            CaseId = viewModel.CaseId
        }, cancellationToken);

        var getUserIdByUserNameDto = await _agentService.GetUserIdByUserName(new GetUserIdByUserNameVm()
        {
            UserName = getUserNameByCaseIdDto.UserName!
        }, cancellationToken);

        var getRadNumberByCaseIdDto = await _agentService.GetRadNumberByCaseId(new GetRadNumberByCaseIdVm()
        {
            CaseId = viewModel.CaseId
        }, cancellationToken);

        var result = await _agentService.RemoveAgentFromCartable(
            new AbortCasesVm()
            {
                userName = getUserNameByCaseIdDto.UserName!,
                userId = getUserIdByUserNameDto.UserId.ToString(),
                Cases = new Shared.Templates.Bizagi.WorkflowEngine.AbortCases.Cases()
                {
                    Enities = new List<Shared.Templates.Bizagi.WorkflowEngine.AbortCases.Case>()
                    {
                        new()
                        {
                            radNumber = getRadNumberByCaseIdDto.RadNumber.ToString(),
                            abortReason = ""
                        }
                    }
                }
            },
            cancellationToken);

        await _agentService.LcpAgentAbortCase(
            viewModel.CaseId,
            cancellationToken);

        return !result.IsRemoved ?
            StatusCode(500, new
            {
                message = result.ErrorMessage ?? "در عملیات حذف نماینده از کارتابل مشکلی رخ داده است"
            }) : Ok(result);
    }
}