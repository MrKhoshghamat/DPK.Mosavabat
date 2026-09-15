using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.Dictionaries.LCP;
using DPK.Mosavabat.Shared.Enums.LCP;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using static System.Int32;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
public class RequestFacilitiesController : ControllerBase
{
    //private readonly IServiceManager _service;
    private readonly IRequestFacilityService _requestFacilityService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly Dictionary<Errors, string>? _errorsRepository;

    public RequestFacilitiesController(
        IRequestFacilityService requestFacilityService,
        ILoggerManager logger,
        IMapper mapper)
    {
        _requestFacilityService = requestFacilityService;
        _logger = logger;
        _mapper = mapper;
        _errorsRepository = ErrorsDictionary.EDictionary;
    }

    /// <summary>
    /// ثبت درخواست تسهیلات اولیه
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "LegalCustomerPortalAdmin")]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> Create(RequestFacilityUpdateDataModel dataModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var accountCheckResult = await _requestFacilityService.CheckCustomerAccountExistsAsync(
            new CheckCustomerAccountExistsDm { AccountNumber = dataModel.AccountNumber },
            cancellationToken);

        if (!accountCheckResult.Exists)
        {
            return Ok(new AjaxResponseBase<MosavabatResponseDataModel?>
            {
                Success = false,
                Error = new ErrorInfo
                {
                    Code = 404,
                    Message = $"شماره حساب وارد شده ({dataModel.AccountNumber}) در سیستم یافت نشد. امکان ثبت درخواست وجود ندارد."
                }
            });
        }

        // ۲. در صورت وجود شماره حساب، فرآیند اصلی ثبت تسهیلات ادامه می‌یابد
        var result = await _requestFacilityService.RegisterRequestFacilityAsyncV2(dataModel);

        var response = new AjaxResponseBase<MosavabatResponseDataModel?>
        {
            Result = result,
            Success = result != null
        };

        return Ok(response);
    }


    /// <summary>
    /// به روز رسانی وضعیت درخواست
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "LegalCustomerPortalAdmin")]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> Update(RequestFacilityUpdateDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        // مدل ویرایش شده از سمت کاربر دریافت شود
        var result = await _requestFacilityService.NewUpdateRequestFacilityV2(dataModel);
        var response = new AjaxResponseBase<MosavabatResponseDataModel?>
        {
            Result = result,
            Success = result != null
        };

        response.Result!.CaseId = Convert.ToInt32(dataModel.CaseId);

        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> UpdateLcpRequestRadNumber(UpdateLcpRequestRadNumberVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var datamodel = _mapper.Map<UpdateLcpRequestRadNumberDataModel>(viewModel);
        var result = await _requestFacilityService.UpdateLcpRequestRadNumber(datamodel);
        var response = new AjaxResponseBase<bool>
        {
            Result = result.IsUpdated,
            Success = true
        };

        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> RemoveRequestFromCartable(
        [FromBody] RemoveLcpRequestFromCartableVm viewModel,
        CancellationToken cancellationToken = default)
    {
        if (viewModel is not { CaseId: > 0 })
            return BadRequest("caseId نامعتبر است");

        var getUserNameByCaseIdDto = await _requestFacilityService.GetUserNameByCaseId(new GetUserNameByCaseIdVm()
        {
            CaseId = viewModel.CaseId
        }, cancellationToken);

        var getUserIdByUserNameDto = await _requestFacilityService.GetUserIdByUserName(new GetUserIdByUserNameVm()
        {
            UserName = getUserNameByCaseIdDto.UserName!
        }, cancellationToken);

        var getRadNumberByCaseIdDto = await _requestFacilityService.GetRadNumberByCaseId(new GetRadNumberByCaseIdVm()
        {
            CaseId = viewModel.CaseId
        }, cancellationToken);

        var result = await _requestFacilityService.RemoveRequestFromCartable(
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

        var lcpRequestFacilityAbortCaseVm = _mapper.Map<LcpRequestFacilityAbortCaseVm>(viewModel);
        await _requestFacilityService.LcpRequestFacilityAbortCase(lcpRequestFacilityAbortCaseVm, cancellationToken);

        return !result.IsRemoved ?
            StatusCode(500, new
            {
                message = result.ErrorMessage ?? "در عملیات حذف درخواست از کارتابل مشکلی رخ داده است"
            }) : Ok(result);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> GetCustomerDebtDetails(GetCustomerDebtDetailsDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var response = await _requestFacilityService.GetCustomerDebtDetails(dataModel);
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> CalculateRequestFacilityUnusedBalance(CalculateRequestFacilityUnusedBalanceDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var response = await _requestFacilityService.CalculateRequestFacilityUnusedBalance(dataModel);
        if (response.Amount  == null)
        {
            return Ok(new AjaxResponseBase<string>
            {
                Result = null,
                Success = false,
                Error = new ErrorInfo
                {
                    Code = 404,
                    Message = "درخواست یافت نشد"
                }
            });

        }
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> PaymentPermissionIssuanceControlForLcp(PaymentPermissionIssuanceControlForLcpDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var response = await _requestFacilityService.PaymentPermissionIssuanceControlForLcp(dataModel);
        var error = CheckError(response.Result);
        return error != null
            ? Ok(CreateResponse<string>(error: error))
            : Ok(CreateResponse(response.Result == "1"));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> SendApprovalLetter(SendApprovalLetterVm vm, CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(vm));

        var response = await _requestFacilityService.SendApprovalLetter(vm, cancellationToken);

        if (string.IsNullOrWhiteSpace(response.Response))
        {
            return Ok(CreateResponse<string>(error: new ErrorInfo
            {
                Code = -1,
                Message = "پاسخی از سرویس دریافت نشد."
            }));
        }

        var rawResponse = response.Response.Trim();

        if (rawResponse.StartsWith("{") && rawResponse.EndsWith("}"))
        {
            try
            {
                var parsedObj = JObject.Parse(rawResponse);
                bool isSuccess = parsedObj["success"]?.Value<bool>() ?? false;

                if (isSuccess)
                {
                    // مقدار result (در اینجا guid برگردانده شده) را در قالب موفق برمی‌گردانیم
                    var resultValue = parsedObj["result"]?.ToString();
                    return Ok(CreateResponse(result: resultValue));
                }
                else
                {
                    // استخراج متن خطا از آبجکت JSON در صورت عدم موفقیت
                    var errorObj = parsedObj["error"];
                    var errorMessage = errorObj?["message"]?.ToString()
                                       ?? errorObj?["details"]?.ToString()
                                       ?? rawResponse;

                    return Ok(CreateResponse<string>(error: new ErrorInfo
                    {
                        Code = errorObj?["code"]?.Value<int>() ?? 0,
                        Message = errorMessage
                    }));
                }
            }
            catch (JsonException)
            {
                // در صورت بروز خطا در Parse، روال عادی پایین ادامه می‌یابد
            }
        }

        var error = CheckError(rawResponse);
        if (error != null)
        {
            return Ok(CreateResponse<string>(error: error));
        }

        if (rawResponse.Equals("true", StringComparison.OrdinalIgnoreCase) || rawResponse == "1")
        {
            return Ok(CreateResponse(result: "true"));
        }

        return Ok(CreateResponse<string>(error: new ErrorInfo
        {
            Code = 0,
            Message = rawResponse
        }));
    }


    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-lcp-v1")]
    [SwaggerOperation(Tags = new[] { "Request Facilities" })]
    public async Task<IActionResult> GetMosavabeExpireDate(GetMosavabeExpireDateDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var viewModel = _mapper.Map<GetMosavabeExpireDateVm>(dataModel);
        var response = await _requestFacilityService.GetMosavabeExpireDate(viewModel);
        if (response.ExpireDate == null)
        {
            return Ok(new AjaxResponseBase<string>
            {
                Result = null,
                Success = false,
                Error = new ErrorInfo
                {
                    Code = 404,
                    Message = "درخواست یافت نشد"
                }
            });

        }
        return Ok(CreateResponse(response));
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

    private ErrorInfo? CheckError(string? result)
    {
        //if (result is not string) return null;
        if (result == null || result.Length > 5 || result == "true" || result == "false") return null;

        var code = Parse(result);

        var errorInfo = new ErrorInfo();
        if (_errorsRepository == null || !_errorsRepository.ContainsKey((Errors)code)) return null;

        errorInfo.Code = code;

        errorInfo.Message = _errorsRepository[(Errors)code];

        return errorInfo;
    }

    ///// <summary>
    ///// ثبت درخواست تسهیلات نسخه جدید
    ///// </summary>
    ///// <returns></returns>
    //[HttpPost]
    ////[Authorize(Roles = "LegalCustomerPortalAdmin")]
    //[ApiExplorerSettings(GroupName = "for-lcp-v2")]
    //[SwaggerOperation(Tags = new[] { "Request Facilities" })]
    //public async Task<IActionResult> CreateRequestFacilityV2(RequestFacilityUpdateDataModel dataModel)
    //{
    //    _logger.Trace(JsonConvert.SerializeObject(dataModel));
    //    var result = await _requestFacilityService.RegisterRequestFacilityAsyncV2(dataModel);

    //    var response = new AjaxResponseBase<MosavabatResponseDataModel?>
    //    {
    //        Result = result,
    //        Success = result != null
    //    };

    //    return Ok(response);
    //}

    ///// <summary>
    ///// به روز رسانی درخواست تسهیلات نسخه جدید
    ///// </summary>
    ///// <returns></returns>
    //[HttpPost]
    ////[Authorize(Roles = "LegalCustomerPortalAdmin")]
    //[ApiExplorerSettings(GroupName = "for-lcp-v2")]
    //[SwaggerOperation(Tags = new[] { "Request Facilities" })]
    //public async Task<IActionResult> UpdateRequestFacilityV2(RequestFacilityUpdateDataModel dataModel)
    //{
    //    _logger.Trace(JsonConvert.SerializeObject(dataModel));
    //    // مدل ویرایش شده از سمت کاربر دریافت شود
    //    var result = await _requestFacilityService.NewUpdateRequestFacilityV2(dataModel);
    //    var response = new AjaxResponseBase<MosavabatResponseDataModel?>
    //    {
    //        Result = result,
    //        Success = result != null
    //    };

    //    return Ok(response);
    //}

    ///// <summary>
    ///// دریافت درخواست تسهیلات نسخه جدید
    ///// </summary>
    ///// <returns></returns>
    //[HttpPost]
    ////[Authorize(Roles = "LegalCustomerPortalAdmin")]
    //[ApiExplorerSettings(GroupName = "for-lcp-v2")]
    //[SwaggerOperation(Tags = new[] { "Request Facilities" })]
    //public async Task<IActionResult> GetLcpRequestFacilityV2(FilterRequestFacilityDataModel filterRequestFacility)
    //{
    //    // مدل ویرایش شده از سمت کاربر دریافت شود
    //    var result = await _requestFacilityService.GetEntitiesUsingSchemaAsyncV2(filterRequestFacility);

    //    var response = new AjaxResponseBase<string?>
    //    {
    //        Result = result,
    //        Success = result != null
    //    };

    //    return Ok(response);
    //}
}