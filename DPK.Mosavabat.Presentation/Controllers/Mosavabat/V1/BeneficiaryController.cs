using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class BeneficiaryController : ControllerBase
{
    private readonly IBeneficiaryService _beneficiaryService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetBeneficiariesDebtListDataModel> _validator;
    private readonly ILoggerManager _logger;

    public BeneficiaryController(
        IBeneficiaryService beneficiaryService,
        IMapper mapper,
        IValidator<GetBeneficiariesDebtListDataModel> validator,
        ILoggerManager logger)
    {
        _beneficiaryService = beneficiaryService;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    /// <summary>
    /// دریافت افراد ذی‌نفع
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Beneficiary" })]
    public async Task<IActionResult> GetBeneficiariesDebtList(GetBeneficiariesDebtListDataModel dataModel)
    {
        var validationResult = await _validator.ValidateAsync(dataModel);
        if (!validationResult.IsValid)
        {
            var checkError = new ErrorInfo
            {
                Message = "مقادیر ورودی معتبر نمی‌باشد"
            };
            return Ok(CreateResponse<string>(error: checkError));
        }

        var viewModel = _mapper.Map<GetBeneficiariesDebtListVm>(dataModel);
        var result = await _beneficiaryService.GetBeneficiariesDebtList(viewModel);
        return Ok(CreateResponse(result));
    }

    private AjaxResponseBase<T> CreateResponse<T>(T? result = default, ErrorInfo? error = null)
    {
        var response = new AjaxResponseBase<T>
        {
            Result = result,
            Success = error == null,
            Error = error
        };

        _logger.Trace(JsonConvert.SerializeObject(response));

        return response;
    }


}