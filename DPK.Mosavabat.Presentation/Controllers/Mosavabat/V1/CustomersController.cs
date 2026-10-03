using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bsi;
using DPK.Mosavabat.Shared.DataModels.Bsi;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "bsi-v1")]
[Route("api/bsi/[controller]/[action]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly ICustomerBsiExternalService _customerBsiExternalService;
    private readonly IMapper _mapper;
    public CustomersController(
        ILoggerManager logger,
        ICustomerBsiExternalService customerBsiExternalService,
        IMapper mapper)
    {
        _logger = logger;
        _customerBsiExternalService = customerBsiExternalService;
        _mapper = mapper;
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "bsi-v1")]
    [SwaggerOperation(Tags = new[] { "Customers" })]
    public async Task<IActionResult> Get(GetCustomerVm vm, CancellationToken cancellationToken = default)
    {
        var bsiCustomer = await _customerBsiExternalService.GetCustomer(
            _mapper.Map<BsiCustomerDataModel>(vm),
            cancellationToken);

        var bsiBalance = await _customerBsiExternalService.GetBalance(
            _mapper.Map<BsiBalanceRequestDataModel>(vm),
            cancellationToken);

        return new CustomerDto()
        {
            CustomerType = 
        }
    }

    // ساخت پاسخ
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