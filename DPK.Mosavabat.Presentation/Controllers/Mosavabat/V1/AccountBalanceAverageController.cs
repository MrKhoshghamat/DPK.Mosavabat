using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Presentation.Main;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
// Todo.H.M : This Controller Is Under Development (Must Be Tested)
public class AccountBalanceAverageController : CustomController
{
    private readonly IAccountBalanceAverageService _accountBalanceAverageService;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public AccountBalanceAverageController(
        IAccountBalanceAverageService accountBalanceAverageService,
        ILoggerManager logger,
        IMapper mapper) : base(logger)
    {
        _accountBalanceAverageService = accountBalanceAverageService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// دریافت معدل موجودی
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Account Balance Average" })]
    public async Task<IActionResult> GetAccountBalanceAverage(AccountBalanceAverageDataModel dataModel) // Method to get account balance average
    {
        var viewModel = _mapper.Map<GetAccountBalanceAverageVm>(dataModel); // Mapping data model to view model
        return await ProcessRequest(viewModel, async () => await _accountBalanceAverageService.GetAccountBalanceAverage(viewModel), "GetAccountBalanceAverage"); // Processing request and returning result
    }

    /// <summary>
    /// دریافت معدل موجودی فرد وابسته
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Account Balance Average" })]
    public async Task<IActionResult> GetRelatedAccountBalanceAverage(GetRelatedAccountBalanceAverageDataModel dataModel) // Method to get account balance average
    {
        _logger.Trace(JsonConvert.SerializeObject("Run GetRelatedAccountBalanceAverage Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var viewModel = _mapper.Map<GetRelatedAccountBalanceAverageVM>(dataModel);
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var response = await _accountBalanceAverageService.GetRelatedAccountBalanceAverage(viewModel);
        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetRelatedAccountBalanceAverage Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        return Ok(CreateResponse(response));
    }

    /// <summary>
    /// مصرف مبلغ مصوب از محل اعتبارات معدل موجودی و باقی مانده حساب
    /// </summary>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Account Balance Average" })]
    public async Task<IActionResult> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsDataModel dataModel) // Method to provide approved amount from credits
    {
        var viewModel = _mapper.Map<ProvidingApprovedAmountFromCreditsVm>(dataModel); // Mapping data model to view model
        return await ProcessRequest(viewModel, async () => await _accountBalanceAverageService.ProvidingApprovedAmountFromCredits(viewModel), "ProvidingApprovedAmountFromCredits"); // Processing request and returning result
    }

    /// <summary>
    /// دریافت معدل موجودی طرح جاری زندگی
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new [] { "Account Balance Average" })]
    public async Task<IActionResult> GetAccountBalanceAverageTarhJariZendegi(GetAccountBalanceAverageTarhJariZendegiDataModel dataModel)
    {
        var viewModel = _mapper.Map<GetAccountBalanceAverageTarhJariZendegiVm>(dataModel);
        return await ProcessRequest(viewModel, async () => await _accountBalanceAverageService.GetAccountBalanceAverageTarhJariZendegi(viewModel), "GetAccountBalanceAverageTarhJariZendegi");
    }

    /// <summary>
    /// دریافت معدل موجودی حسابهای مشترک
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Account Balance Average" })]
    public async Task<IActionResult> GetSharedAccountBalanceAverage(GetSharedAccountBalanceAverageDataModel dataModel)
    {
        var viewModel = _mapper.Map<GetSharedAccountBalanceAverageVM>(dataModel);
        return await ProcessRequest(viewModel, async () => await _accountBalanceAverageService.GetSharedAccountBalanceAverage(viewModel), "GetSharedAccountBalanceAverage");
    }

    [HttpPost]
    [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
    [SwaggerOperation(Tags = new[] { "Account Balance Average" })]
    public async Task<IActionResult> GetRawAccountBalanceAverage(GetRawAccountBalanceAverageVm vm,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        _logger.Trace(JsonConvert.SerializeObject("Run GetRawAccountBalanceAverage Service From MA"));
        _logger.Trace(JsonConvert.SerializeObject(vm));
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await _accountBalanceAverageService.GetRawAccountBalanceAverage(vm, cancellationToken);

        stopwatch.Stop();
        _logger.Trace(JsonConvert.SerializeObject($"GetRawAccountBalanceAverage Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));

        return Ok(CreateResponse(response));
    }
}
