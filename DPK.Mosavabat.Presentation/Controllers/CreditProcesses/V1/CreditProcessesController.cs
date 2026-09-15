using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.Dictionaries.CreditProcesses;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.Enums.CreditProcesses;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Payam;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using static System.Int32;
using IContractService = DPK.Mosavabat.Services.Contracts.Mosavabat.IContractService;

namespace DPK.Mosavabat.Presentation.Controllers.CreditProcesses.V1;

//[Authorize]
[ApiVersion("1.0")]
[Route("api/[controller]/[action]")]
[ApiController]
public class CreditProcessesController : ControllerBase
{
    private readonly Dictionary<Errors, string>? _errorsRepository;
    private readonly ILoanExternalService _loanExternalService;
    private readonly ILmsService _lmsService;
    private readonly ICreditProcessService _creditProcessService;
    private readonly ICreditProcessesDatabaseService _creditProcessesDatabaseService;
    private readonly IContractService _contractService;
    private readonly IUserDatabase _userDatabase;
    private readonly ISamatExternalService _samatExternalService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;


    public CreditProcessesController(
        ILoanExternalService loanExternalService,
        ICreditProcessService creditProcessService,
        ICreditProcessesDatabaseService creditProcessesDatabaseService,
        IContractService contractService,
        IUserDatabase userDatabase,
        ILmsService lmsService,
        ILoggerManager logger,
        IMapper mapper,
        ISamatExternalService samatExternalService)
    {
        _loanExternalService = loanExternalService;
        _creditProcessService = creditProcessService;
        _creditProcessesDatabaseService = creditProcessesDatabaseService;
        _contractService = contractService;
        _userDatabase = userDatabase;
        _lmsService = lmsService;
        _logger = logger;
        _mapper = mapper;
        _samatExternalService = samatExternalService;
        _errorsRepository = ErrorsDictionary.EDictionary;
    }

    // سرویس اول 
    /// <summary>
    /// [1] [کنترل امکان صدور مجوز پرداخت]
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run PaymentPermissionIssuanceControl Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };
            return Ok(CreateResponse<string>(error: checkError));
        }
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        //var paymentPermissionIssuanceControlVm = _mapper.Map<PaymentPermissionIssuanceControlVm>(dataModel);

        var paymentPermissionIssuanceControlVm = new PaymentPermissionIssuanceControlVm()
        {
            ContractAmount = Convert.ToInt64(dataModel.ContractAmount),
            CustomerSharePrice = Convert.ToInt64(dataModel.CustomerSharePrice),
            CustomerNo = dataModel.CustomerNo,
            MosavabeNo = dataModel.MosavabeNumber,
            SubContractType = dataModel.SubContractType
        };

        // در صورتی که کنترل موفق باشد یک کد ملی برگشت داده می شود
        var paymentPermissionIssuanceControlResponse = await _creditProcessService.PaymentPermissionIssuanceControl(paymentPermissionIssuanceControlVm);


        var error = CheckError(paymentPermissionIssuanceControlResponse!.Result!);
        if (error != null) return Ok(CreateResponse<string>(error: error));

        var estelamCheckVm = _mapper.Map<EstelamCheckVm>(paymentPermissionIssuanceControlResponse);

        // کد ملی برای استعلام چک برگشتی به سرویس استعلام چک پاس داده می شود
        var estelamCheckResponse = await _samatExternalService.EstelamCheck(estelamCheckVm);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"PaymentPermissionIssuanceControl Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(estelamCheckResponse.BouncedCheckCount == "0"));
    }

    // سرویس سوم
    /// <summary>
    /// [3] [دریافت لیست مفاد و شروط مصوبه]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetListTermsAndConditionsApprovalRequest(GetListTermsAndConditionsApprovalRequestDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetListTermsAndConditionsApprovalRequest Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        //todo m.khoshghamat : not implement to lms.
        var getTermsAndConditionsApprovalRequestListVm = _mapper.Map<GetTermsAndConditionsApprovalRequestListVm>(dataModel);
        var getTermsAndConditionsApprovalRequestsResponse = await _creditProcessService.GetTermsAndConditionsApprovalRequests(getTermsAndConditionsApprovalRequestListVm);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetListTermsAndConditionsApprovalRequest Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(getTermsAndConditionsApprovalRequestsResponse));
    }

    // سرویس چهارم
    /// <summary>
    /// [4] [ورود اطلاعات صدور مجوز پرداخت]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> InsertPaymentPermission(InsertPaymentPermissionDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run InsertPaymentPermission Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"InsertPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        //var paymentPermissionIssuanceControlVm = _mapper.Map<PaymentPermissionIssuanceControlVm>(dataModel);

        var paymentPermissionIssuanceControlVm = new PaymentPermissionIssuanceControlVm()
        {
            ContractAmount = Convert.ToInt64(dataModel.ContractAmount),
            CustomerSharePrice = Convert.ToInt64(dataModel.PrepaidAmount),
            CustomerNo = dataModel.CustomerNo,
            MosavabeNo = dataModel.MosavabeNo,
            SubContractType = dataModel.SubContractType
        };

        // در ابتدا قبل از ایجاد مجوز کنترل پرداخت جدید باید کنترل شرط های صدور مجوز انجام شود
        var paymentPermissionIssuanceControlResponse = await _creditProcessService.PaymentPermissionIssuanceControl(paymentPermissionIssuanceControlVm);

        if (paymentPermissionIssuanceControlResponse!.Result == "1114")
        {
            var calculateApprovalRequestUnusedBalance = await _creditProcessService.CalculateApprovalRequestUnusedBalance(new CalculateApprovalRequestUnusedBalanceVm()
            {
                MosavabeNumber = dataModel.MosavabeNo
            });

            if (calculateApprovalRequestUnusedBalance!.RemainingContractsCount == 0)
            {
                var paymentPermissionIssuanceControlResponseError = CheckError(paymentPermissionIssuanceControlResponse.Result);
                if (paymentPermissionIssuanceControlResponseError != null)
                    return Ok(CreateResponse<string>(error: paymentPermissionIssuanceControlResponseError));
            }
        }

        // کد ملی برای استعلام چک برگشتی به سرویس استعلام چک پاس داده می شود
        var estelamCheckVm = _mapper.Map<EstelamCheckVm>(paymentPermissionIssuanceControlResponse);
        var estelamCheckResponse = await _samatExternalService.EstelamCheck(estelamCheckVm);

        _logger.Trace(JsonConvert.SerializeObject(estelamCheckResponse));
        if (Parse(estelamCheckResponse.BouncedCheckCount) != 0)
        {
            var checkError = new ErrorInfo
            {
                Code = 1109,
                Message = "دارای چک برگشتی می باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"InsertPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }

        var lmsInsertPaymentPermissionResponse = await _lmsService.InsertPaymentPermission(dataModel);

        if (!lmsInsertPaymentPermissionResponse.IsAuthenticate)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"InsertPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnAuthorizeResponse();
        }
        if (lmsInsertPaymentPermissionResponse.Result.Error != null)
        {
            var lmsInsertPaymentPermissionResponseError = CheckError(lmsInsertPaymentPermissionResponse.Result.Error);
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"InsertPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: lmsInsertPaymentPermissionResponseError));
        }

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"InsertPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(lmsInsertPaymentPermissionResponse.Success));
    }

    // سرویس پنجم
    /// <summary>
    /// [5] [ایجاد یک نمونه از فرآیند تکمیل اطلاعات قرارداد]
    /// </summary>
    [HttpPost]
    //[Authorize(Roles = "LoanManagementSystemAdmin")]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> CreateCompleteContractInfoProcess(CreateCompleteContractInfoProcessDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run CreateCompleteContractInfoProcess Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();


        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"CheckExistActivePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        var calculateApprovalRequestUnusedBalanceVm = _mapper.Map<CreateCompleteContractInfoProcessVm>(dataModel);
        var result = await _creditProcessService.CreateCompleteContractInfoProcess(calculateApprovalRequestUnusedBalanceVm);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"CheckExistActivePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(result));
    }

    // سرویس ششم
    /// <summary>
    /// [6] [محاسبه مانده آزاد مصوبه]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run CalculateApprovalRequestUnusedBalance Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"CalculateApprovalRequestUnusedBalance Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        var calculateApprovalRequestUnusedBalanceVm = _mapper.Map<CalculateApprovalRequestUnusedBalanceVm>(dataModel);
        var result = await _creditProcessService.CalculateApprovalRequestUnusedBalance(calculateApprovalRequestUnusedBalanceVm, Request.HttpContext.RequestAborted);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"CalculateApprovalRequestUnusedBalance Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(result));
    }

    // سرویس هشتم
    /// <summary>
    /// [8] [محاسبه مانده بدهی مشتری]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> CalculationCustomerDebtBalance(CalculationCustomerDebtBalanceDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run CheckExistActivePaymentPermission Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"CalculationCustomerDebtBalance Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"CalculationCustomerDebtBalance Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }

        var calculateCustomerDebtBalanceVm = _mapper.Map<CalculateCustomerDebtBalanceVm>(dataModel);
        var recalculateCustomerDebtBalanceResponse = await _creditProcessService.CalculateCustomerDebtBalance(calculateCustomerDebtBalanceVm);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"CalculationCustomerDebtBalance Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(recalculateCustomerDebtBalanceResponse!.Result));
    }

    // سرویس نهم
    /// <summary>
    /// [9] [آپدیت وضعیت پرداخت]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> UpdatePaymentPermission(UpdatePaymentPermissionDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run UpdatePaymentPermission Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();


        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"UpdatePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"UpdatePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);

        }

        if (dataModel is { IsCancel: true, IsDelete: true })
        {
            var checkError = CheckError(Errors.CodeDeleteAndCancelCanNotBothBeActive);
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"UpdatePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }

        var updatePaymentPermissionResponse = await _lmsService.UpdatePaymentPermission(dataModel);

        if (dataModel.IsCancel)
        {
            var updateIsDeletedCuContractResult = await _contractService.UpdateIsDeletedCuContract(new Shared.DataModels.Mosavabat.Contract.UpdateIsDeletedCuContractDm()
            {
                PaymentPermissionId = dataModel.Id,
                IsDeleted = true
            });

            if (!updateIsDeletedCuContractResult.IsUpdated)
            {
                stopwatch.Stop();
                _logger.Trace(JsonConvert.SerializeObject($"UpdateIsDeletedCuContract Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            }
        }

        if (!updatePaymentPermissionResponse.IsAuthenticate)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"UpdatePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnAuthorizeResponse();
        }

        if (updatePaymentPermissionResponse.Result.Error.HasValue)
        {
            var responseError = updatePaymentPermissionResponse.Result.Error;
            var error = CheckError(responseError);

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"UpdatePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: error));

        }

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"UpdatePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(updatePaymentPermissionResponse.Success));
    }

    // سرویس دهم
    /// <summary>
    /// [10] [دریافت لیست پرداخت ها]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetListOfPaymentPermission(GetPaymentPermissionListDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetListOfPaymentPermission Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetListOfPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetListOfPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }

        var viewModel = _mapper.Map<GetPaymentPermissionListDataModel>(dataModel);
        var result = await _lmsService.FetchPaymentPermissions(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetListOfPaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(result);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetContracts(GetContractsVm? viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetContracts Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (viewModel == null)
        {
            var checkError = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetContracts Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetContracts Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }

        var getPaymentPermissionListDataModel = _mapper.Map<GetPaymentPermissionListDataModel>(viewModel);
        var getPaymentPermissionListResponse = await _lmsService.FetchPaymentPermissions(getPaymentPermissionListDataModel);

        var getCuContractsViewModel = _mapper.Map<GetCuContractsVm>(viewModel);
        var getCuContractsResponse = await _creditProcessService.GetCuContracts(getCuContractsViewModel);

        List<GetContractsDto?> result = getPaymentPermissionListResponse.Result.Select(paymentPermission => _mapper.Map<GetContractsDto>(paymentPermission)).ToList()!;
        result.AddRange(getCuContractsResponse.Select(cuContract => _mapper.Map<GetContractsDto>(cuContract)));

        result = result
            .GroupBy(x => x?.C13)
            .Select(x => x.FirstOrDefault())
            .ToList();

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetContracts Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(result));
    }

    // سرویس یازدهم
    /// <summary>
    /// [11] [استعلام چک]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> EstelamCheck(EstelamCheckDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run EstelamCheck Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError1 = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"EstelamCheck Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError1));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"EstelamCheck Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }
        var estelamCheckVm = _mapper.Map<EstelamCheckVm>(dataModel);
        var estelamCheckResponse = await _samatExternalService.EstelamCheck(estelamCheckVm);

        var checkStatus = Parse(estelamCheckResponse.BouncedCheckCount) != 0;
        var checkError = new ErrorInfo
        {
            Code = 1109,
            Message = "دارای چک برگشتی می باشد"
        };

        if (checkStatus)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"EstelamCheck Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError));
        }

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"EstelamCheck Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(checkStatus));
    }

    /// <summary>
    /// دریافت لیست تسهیلات و تعهدات در شبکه بانکی
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetCbiTasList(CbiTasVm? viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetCbiTasList Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (viewModel == null)
        {
            var checkError1 = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetCbiTasList Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError1));
        }
        // اعتبار سنجی
        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetCbiTasList Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }
        var getCbiTasListResponse = await _samatExternalService.GetCbiTas(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"EstelamCheck Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(getCbiTasListResponse));
    }

    /// <summary>
    /// دریافت وضعیت دارا بودن بدهی غیر جاری
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> HasNonCurrentDebt(CbiTasVm? viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run HasNonCurrentDebt Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (viewModel == null)
        {
            var checkError1 = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل مجوز پرداخت صحیح نمی باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"HasNonCurrentDebt Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError1));
        }

        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"HasNonCurrentDebt Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }

        var hasNonCurrentDebtDto = await _samatExternalService.HasNonCurrentDebt(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"HasNonCurrentDebt Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(hasNonCurrentDebtDto.HasNonCurrentDebt));
    }

    // سرویس دوازدهم
    /// <summary>
    /// [12] [بررسی مجوز کنترل پرداخت]
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> CheckExistActivePaymentPermission(CheckExistActivePaymentPermissionDataModel? dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run CheckExistActivePaymentPermission Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (dataModel == null)
        {
            var checkError1 = new ErrorInfo
            {
                Code = 0001,
                Message = "فرمت مدل ورودی اشتباه می باشد"
            };

            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"CheckExistActivePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return Ok(CreateResponse<string>(error: checkError1));
        }

        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"CheckExistActivePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }

        var checkExistActivePaymentPermissionVm = _mapper.Map<CheckExistActivePaymentPermissionVm>(dataModel);

        var result = await _creditProcessService.CheckExistActivePaymentPermission(checkExistActivePaymentPermissionVm);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"CheckExistActivePaymentPermission Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(result is { Result: true }));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-abrisham-v1")]
    [SwaggerOperation(Tags = new[] { "Abrisham" })]
    public async Task<IActionResult> GetLoanInfo(GetLoanInfoVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetLoanInfo Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();


        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetLoanInfo Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }
        var response = await _loanExternalService.GetLoanInfo(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetLoanInfo Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-abrisham-v1")]
    [SwaggerOperation(Tags = new[] { "Abrisham" })]
    public async Task<IActionResult> GetLoanBill(GetLoanBillVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetLoanBill Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();


        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetLoanBill Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }
        var response = await _loanExternalService.GetLoanBill(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetLoanBill Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-abrisham-v1")]
    [SwaggerOperation(Tags = new[] { "Abrisham" })]
    public async Task<IActionResult> GetLoanContractsAllStatement(GetLoanContractsAllStatementVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetLoanContractsAllStatement Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetLoanContractsAllStatement Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }

        var response = await _loanExternalService.GetLoanContractsAllStatement(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetLoanContractsAllStatement Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-abrisham-v1")]
    [SwaggerOperation(Tags = new[] { "Abrisham" })]
    public async Task<IActionResult> GetLoanCustomerFacilities(GetLoanCustomerFacilitiesVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetLoanCustomerFacilities Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();


        if (!ModelState.IsValid)
        {
            stopwatch.Stop();
            _logger.Trace(JsonConvert.SerializeObject($"GetLoanCustomerFacilities Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            return UnprocessableEntity(ModelState);
        }
        var response = await _loanExternalService.GetLoanCustomerFacilities(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetLoanCustomerFacilities Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-abrisham-v1")]
    [SwaggerOperation(Tags = new[] { "Abrisham" })]
    public async Task<IActionResult> GetLoanBillReport(GetLoanBillReportVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetLoanBillReport Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();


        var response = await _creditProcessService.GetLoanBillReport(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetLoanBillReport Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> AggregateDailyCustomerPayment(AggregateDailyCustomerPaymentVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run AggregateDailyCustomerPayment Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.AggregateDailyCustomerPayment(viewModel, Request.HttpContext.RequestAborted);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"AggregateDailyCustomerPayment Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> CalculateCustomerTotalDebt(CalculateCustomerTotalDebtVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run CalculateCustomerTotalDebt Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.CalculateCustomerTotalDebt(viewModel, Request.HttpContext.RequestAborted);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"CalculateCustomerTotalDebt Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsAmountsSumVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetLiveApprovalRequestsAmountsSum Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetLiveApprovalRequestsAmountsSum(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetLiveApprovalRequestsAmountsSum Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> IsLcpRequest(IsLcpRequestVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run IsLcpRequest Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.IsLcpRequest(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"IsLcpRequest Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    /// <summary>
    /// سرویس دریافت لیست دلایل ثبت استثنا برای فرآیند صدور شناسه مجوز پرداخت
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetListOfExceptionPayReason(GetListOfExceptionPayReasonVM viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetListOfExceptionPayReason Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetListOfExceptionPayReason(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetListOfExceptionPayReason Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    /// <summary>
    /// سرویس دریافت لیست دلایل استثنا ثبت شده توسط مشتری
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetListOfPaymentPermissionExceptionReason(GetListOfPaymentPermissionExceptionReasonVM viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetListOfPaymentPermissionExceptionReason Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetListOfPaymentPermissionExceptionReason(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetListOfPaymentPermissionExceptionReason Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    /// <summary>
    /// اضافه کردن استثنا مجوز کنترل پرداخت به یک مشتری خاص
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> AddToPaymentPermissionExceptionReason(PaymentPermissionExceptionReasonVM viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run AddToPaymentPermissionExceptionReason Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.AddToPaymentPermissionExceptionReason(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"AddToPaymentPermissionExceptionReason Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    /// <summary>
    /// حذف کردن استثنا مجوز کنترل پرداخت از یک مشتری خاص
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> DeletePaymentPermissionExceptionReason(PaymentPermissionExceptionReasonForDeleteVM viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run DeletePaymentPermissionExceptionReason Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.DeletePaymentPermissionExceptionReason(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"DeletePaymentPermissionExceptionReason Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    /// <summary>
    /// سرویس دریافت لیست کدهای استثنا ثبت شده برای یک مشتری با جستجوی شناسه مشتری
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetListOfExceptionCodeByNationalCode(GetListOfExceptionCodeByNationalCodeVM viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetListOfExceptionCodeByNationalCode Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetListOfExceptionCodeByNationalCode(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetListOfExceptionCodeByNationalCode Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetDailyPaidContracts(GetDailyPaidContractsVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetDailyPaidContracts Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetDailyPaidContracts(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetDailyPaidContracts Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetCustomerContractsPastDebt(GetCustomerContractsPastDebtVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetCustomerContractsPastDebt Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetCustomerContractsPastDebt(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetCustomerContractsPastDebt Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetSubjectType(GetSubjectTypeVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetSubjectType Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetSubjectType(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetSubjectType Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetApprovalRequestInfo(GetApprovalRequestInfoVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetApprovalRequestInfo Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.GetApprovalRequestInfo(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetApprovalRequestInfo Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> IsAllowedRecalculateAccountBalanceAverage(IsAllowedRecalculateAccountBalanceAverageVm viewModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetApprovalRequestInfo Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _creditProcessService.IsAllowedRecalculateAccountBalanceAverage(viewModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetApprovalRequestInfo Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> RegisterPaymentPermissionExceptionReason(RegisterPaymentPermissionExceptionReasonVm viewModel)
    {
        var dataModel = _mapper.Map<RegisterPaymentPermissionExceptionReasonsDataModel>(viewModel);
        var response = await _lmsService.RegisterPaymentPermissionExceptionReason(dataModel);

        if (response is { Error: { Code: 0, Message: "Method arguments are not valid! See ValidationErrors for details." }, Result: null })
        {
            return Ok(CreateResponse<string>(
                error: new ErrorInfo
                {
                    Code = 1123,
                    Message = "خطا در عملیات ثبت استثنای مجوز پرداخت"
                })
                );
        }

        if (response.Result?.Error == null) return Ok(CreateResponse(response.Success));
        var responseError = CheckError(response.Result.Error);
        return Ok(CreateResponse<string>(error: responseError));

    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> GetPaymentPermissionExceptionReasons(GetPaymentPermissionExceptionReasonsVm viewModel)
    {
        var dataModel = _mapper.Map<GetPaymentPermissionExceptionReasonsDataModel>(viewModel);
        var response = await _lmsService.GetPaymentPermissionExceptionReasons(dataModel);
        return Ok(response);
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "for-credit-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> CheckBoundaryComplianceInquiry(BoundaryComplianceInquiryDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject("Run CheckBoundaryComplianceInquiry Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _lmsService.CheckBoundaryComplianceInquiry(dataModel);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"CheckBoundaryComplianceInquiry Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response.Result));
    }

    [HttpPost("/api/mosavabat/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "CreditProcesses" })]
    public async Task<IActionResult> Create([FromBody] CreatePaymentPermissionDataModel dataModel)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));
            // put a breakpoint here and inspect `errors` in the debugger
        }
        var requestFacilityDetails = await _creditProcessesDatabaseService
            .GetRequestFacilityDetails(new GetRequestFacilityDetailsVm { CaseId = dataModel.RequestFacilityCaseId });

        if (requestFacilityDetails.MosavabeNo == 0)
            return Ok(new AjaxResponseBase<MosavabatResponseDataModel?>
            {
                Success = false,
                Error = new ErrorInfo { Code = 404, Message = "درخواست تسهیلاتی با caseId وارد شده وجود ندارد." }
            });

        var userName = await _userDatabase.GetUserName(
            requestFacilityDetails.BranchCode, MosavabatClassName.LCP_IssuancePayment);

        if (userName is null)
            return Problem("کاربر مرتبط با این شعبه یافت نشد.");

        var userId = await _userDatabase.GetUserId(userName);

        var model = MapToCreateIssuancePaymentModel(dataModel, requestFacilityDetails, userId, userName);

        var result = await _lmsService.CreateIssuancePayment(model);

        return Ok(CreateResponse(new AjaxResponseBase<MosavabatResponseDataModel?>
        {
            Result = result,
            Success = result != null
        }));
    }

    private static CreateIssuancePaymentPermissionDataModel MapToCreateIssuancePaymentModel(
        CreatePaymentPermissionDataModel src,
        GetRequestFacilityDetailsDto facility,
        int userId,
        string userName) => new()
        {
            UserId = userId,
            UserName = userName,
            MosavabeNo = facility.MosavabeNo,
            Customer = facility.Customer,
            BranchId = facility.BranchId,
            ContractAmount = src.ContractAmount,
            BankSharePrice = src.BankSharePrice,
            CustomerCashPrice = src.CustomerCashPrice,
            CustomerNonCashPrice = src.CustomerNonCashPrice,
            Guarantors = src.Guarantors ?? new PayGuarantorsDataModel { Guarantors = new() },
            Collaterals = src.Collaterals ?? new PayCollateralsDataModel { Collaterals = new() },
            Factors = src.Factors ?? new PayBaratFactorsDataModel { Factors = new() },
            Beneficiaries = src.Beneficiaries ?? new PayBaratBeneficiariesDataModel { Beneficiaries = new() },
            CreditRank = src.CreditRank,
            CreditScore = src.CreditScore,
            LcpId = src.LcpId.ToString(),
            Description = src.Description,
            BaratDueDate = src.BaratDueDate,
            FeeAmount = src.FeeAmount
        };

    // بررسی خطای برگشت داده شده از سمت دیتا بیس 
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

    private ErrorInfo CheckError(Errors? errorCods)
    {
        var errorInfo = new ErrorInfo()
        {
            Code = default,
            Message = default
        };

        if (errorCods == null) return errorInfo;
        errorInfo.Code = (int)errorCods;
        errorInfo.Message = _errorsRepository?[errorCods.Value];
        return errorInfo;
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

    // پاسخ نداشتن مجوز
    private IActionResult UnAuthorizeResponse()
    {
        var unAuthorizeResponse = new AjaxResponseBase<LmsResponse>
        {
            Result = null,
            Success = false,
            Error = new ErrorInfo()
            {
                Code = 401,
                Message = ErrorCodes.UnAuthorize,
            }
        };
        return Ok(unAuthorizeResponse);
    }
}