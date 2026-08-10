using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
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
public class ContractController : ControllerBase
{
    //private readonly IServiceManager _service;
    private readonly IContractService _contractService;
    private readonly IRequestFacilityService _requestFacilityService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public ContractController(IContractService contractService, IRequestFacilityService requestFacilityService, ILoggerManager logger, IMapper mapper)
    {
        _contractService = contractService;
        _requestFacilityService = requestFacilityService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// دریافت لینک فایل قرارداد
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Contract" })]
    public async Task<IActionResult> GetContractFileLink(GetMorabeheContractFileLinkDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var viewModel = _mapper.Map<GetContractFileLinkVm>(dataModel);
        var result = await _contractService.GetContractFileLink(viewModel);

        var response = new AjaxResponseBase<string?>
        {
            Result = result!.Result,
            Success = result != null
        };

        return Ok(response);
    }

    /// <summary>
    /// به روز رسانی فایل قرارداد
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "LegalCustomerPortalAdmin")]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Contract" })]
    public async Task<IActionResult> UpdateContractFileLink(UpdateMorabeheContractFileLink dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var viewModel = _mapper.Map<UpdateContractFileLinkVm>(dataModel);
        var result = await _requestFacilityService.UpdateContractFileLink(viewModel);

        var response = new AjaxResponseBase<string?>
        {
            Result = result.Result,
            Success = true
        };

        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Contract" })]
    public async Task<IActionResult> GetApprovalContracts(GetApprovalContractsVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        // H.M.Todo : Check this map
        // Message: Missing type map configuration or unsupported mapping.    Mapping types:  GetApprovalContractsVm -> GetApprovalContractsDm  DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract.GetApprovalContractsVm -> DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract.GetApprovalContractsDm
        var dataModel = _mapper.Map<GetApprovalContractsDm>(viewModel);
        var result = await _contractService.GetApprovalContracts(dataModel);

        var response = new AjaxResponseBase<List<ContractDto>>
        {
            Result = result.Contracts,
            Success = true
        };

        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Contract" })]
    public async Task<IActionResult> GetContractTypeTitleByContractTypeId(GetContractTypeTitleByContractTypeIdVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var dataModel = _mapper.Map<GetContractTypeTitleByContractTypeIdDm>(viewModel);
        var result = await _contractService.GetContractTypeTitleByContractTypeId(dataModel);

        var response = new AjaxResponseBase<GetContractTypeTitleByContractTypeIdDto>
        {
            Result = result,
            Success = true
        };

        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Contract" })]
    public async Task<IActionResult> GetSubContractTypeTitleBySubContractTypeId(GetSubContractTypeTitleBySubContractTypeIdVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var dataModel = _mapper.Map<GetSubContractTypeTitleBySubContractTypeIdDm>(viewModel);
        var result = await _contractService.GetSubContractTypeTitleBySubContractTypeId(dataModel);

        var response = new AjaxResponseBase<GetSubContractTypeTitleBySubContractTypeIdDto>
        {
            Result = result,
            Success = true
        };

        return Ok(response);
    }
}