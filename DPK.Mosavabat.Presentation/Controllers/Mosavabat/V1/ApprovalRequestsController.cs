using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class ApprovalRequestsController : ControllerBase
{
    //private readonly IServiceManager _service;
    private readonly IApprovalRequestService _approvalRequestService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public ApprovalRequestsController(IApprovalRequestService approvalRequestService, ILoggerManager logger, IMapper mapper)
    {
        _approvalRequestService = approvalRequestService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// ثبت درخواست تسهیلات اصلی
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Approval Requests" })]
    public async Task<IActionResult> Create(CreateApprovalRequestDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var viewModel = _mapper.Map<CreateApprovalRequestVm>(dataModel);
        var result = await _approvalRequestService.CreateApprovalRequest(viewModel);

        var response = new AjaxResponseBase<MosavabatResponseDataModel?>
        {
            Result = result!.MosavabatResponseDataModel,
            Success = result != null
        };

        return Ok(response);
    }

    /// <summary>
    /// لیست وضعیت های درخواست تسهیلات اصلی
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "LegalCustomerPortalAdmin")]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Approval Requests" })]
    public async Task<IActionResult> GetAllStatus(GetRequestStatuesDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var viewModel = _mapper.Map<GetRequestStatusVm>(dataModel);
        var result = await _approvalRequestService.GetRequestStatus(viewModel);

        var response = new AjaxResponseBase<List<GetRequestStatusDto>?>
        {
            Result = result,
            Success = result != null
        };

        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Approval Requests" })]
    public async Task<IActionResult> IsExistFacilitySubTypeCodeInGroupCreditPackage(IsExistFacilitySubTypeCodeInGroupCreditPackageVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var result = await _approvalRequestService.IsExistFacilitySubTypeCodeInGroupCreditPackage(viewModel);
        var response = new AjaxResponseBase<bool> { Result = result.IsExist, Success = result != null };
        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Approval Requests" })]
    public async Task<IActionResult> GetRequestsLastStatus(
        [FromBody] GetRequestsLastStatusVm viewModel,
        CancellationToken cancellationToken = default)
    {
        if (viewModel?.CaseIds == null || !viewModel.CaseIds.Any())
        {
            return Ok(new AjaxResponseBase<Dictionary<int, string?>>
            {
                Success = true,
                Result = new Dictionary<int, string?>()
            });
        }

        if (viewModel.CaseIds.Count > 100)
        {
            return BadRequest("Number of case IDs must not exceed 100.");
        }

        var result = 
            await _approvalRequestService.GetRequestsLastStatus(
                viewModel, 
                cancellationToken);

        var response = new AjaxResponseBase<Dictionary<int, string?>>
        {
            Result = result?.Statuses ?? new Dictionary<int, string?>(),
            Success = result != null
        };

        return Ok(response);
    }
}