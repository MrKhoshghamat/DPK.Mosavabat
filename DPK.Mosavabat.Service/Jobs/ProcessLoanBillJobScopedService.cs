using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Entities.Helper;
using DPK.Mosavabat.Entities.Models.LoanBill;
using DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;

namespace DPK.Mosavabat.Services.Jobs;

public class ProcessLoanBillJobScopedService
{
    private readonly ILoanBillRequestRepository _loanBillRequestRepository;
    private readonly ILoanBillDataRepository _loanBillDataRepository;
    private readonly ILoanExternalService _loanExternalService;
    private readonly IMapper _mapper;
    //private readonly ILoggerManager _logger;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    public ProcessLoanBillJobScopedService(
        ILoanBillRequestRepository loanBillRequestRepository,
        ILoanBillDataRepository loanBillDataRepository,
        ILoanExternalService loanExternalService,
        IMapper mapper
        /*ILoggerManager logger*/)
    {
        _loanBillRequestRepository = loanBillRequestRepository;
        _loanBillDataRepository = loanBillDataRepository;
        _loanExternalService = loanExternalService;
        _mapper = mapper;
        //_logger = logger;

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (ex, _) =>
                {
                    //_logger.Warn($"Circuit breaker opened due to {ex.Message}");
                },
                onReset: () =>
                {
                    //_logger.Warn("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    //_logger.Warn("Circuit breaker half-open - next call is a trial");
                });
    }

    public async Task ProcessSingleLoanBillAsync(LoanBillRequest request)
    {
        //var loanId = request.LoanId;
        var step = "Started";
        //_logger.Debug($"Processing LoanId {loanId} - Step: {step}");

        using var processCts = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(processCts.Token);

        try
        {
            // === STEP 1: Mark as processing ===
            //step = "MarkAsProcessing";
            //_logger.Debug($"Processing LoanId {loanId} - Step: {step}");
            request.IsProcessing = true;

            // Explicitly update detached entity
            await _loanBillRequestRepository.UpdateAsync(request, CancellationToken.None);
            await _loanBillRequestRepository.SaveChangesAsync(CancellationToken.None);

            // === STEP 2: Call external service ===
            //step = "CallExternalService";
            //_logger.Debug($"Processing LoanId {loanId} - Step: {step}");
            var loanBill = _mapper.Map<GetLoanBillDto>(await CallExternalServiceWithCircuitBreakerAndRetry_v2(request.LoanId, request.DateUpdate));
            loanBill.LoanId = request.LoanId;

            // === STEP 3: Save loan bill data ===
            //step = "SaveLoanBillData";
            //_logger.Debug($"Processing LoanId {loanId} - Step: {step}");
            await SaveLoanBillData(request, loanBill);

            // === STEP 4: Mark as completed ===
            //step = "MarkAsCompleted";
            //_logger.Debug($"Processing LoanId {loanId} - Step: {step}");

            request.IsCompleted = true;
            request.IsProcessing = false;
            request.CompletedAt = IranTimeHelper.ToIranTimeNow();

            // Explicitly update detached entity
            await _loanBillRequestRepository.UpdateAsync(request, CancellationToken.None);
            await _loanBillRequestRepository.SaveChangesAsync(CancellationToken.None);

            //_logger.Info($"Successfully completed processing for LoanId {loanId}");
        }
        catch (OperationCanceledException ex)
        {
            // Log cancellation source
            var cancellationSource = ex.CancellationToken == processCts.Token
                ? "Overall process timeout"
                : ex.CancellationToken == linkedCts.Token
                    ? "Linked token cancellation"
                    : "External cancellation (Quartz/Bizagi/EF)";

            //_logger.Error($"Processing for LoanId {loanId} was canceled at step '{step}' due to: {cancellationSource}. Message: {ex.Message}");

            // Update retry status
            request.RetryCount++;
            request.ErrorMessage = $"Canceled at step '{step}': {cancellationSource}";

            if (request.RetryCount < 2)
            {
                var nextRetryIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(Math.Pow(2, request.RetryCount));
                request.NextRetryAt = TimeZoneInfo.ConvertTimeToUtc(nextRetryIranTime, IranTimeHelper.IranTimeZone);
                request.IsProcessing = false;
            }
            else
            {
                request.IsCompleted = true; // Mark as completed (failed)
                request.IsProcessing = false;
            }

            // Save updated status
            await _loanBillRequestRepository.UpdateAsync(request, CancellationToken.None);
            await _loanBillRequestRepository.SaveChangesAsync(CancellationToken.None);
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
            //_logger.Error($"Failed to process LoanId {loanId} at step '{step}'. Error: {ex.Message}");

            // Update retry status
            request.RetryCount++;
            request.ErrorMessage = $"Error at step '{step}': {ex.Message}";

            if (request.RetryCount < 2)
            {
                var nextRetryIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(Math.Pow(2, request.RetryCount));
                request.NextRetryAt = TimeZoneInfo.ConvertTimeToUtc(nextRetryIranTime, IranTimeHelper.IranTimeZone);
                request.IsProcessing = false;
            }
            else
            {
                request.IsCompleted = true; // Mark as completed (failed)
                request.IsProcessing = false;
            }

            await _loanBillRequestRepository.UpdateAsync(request, CancellationToken.None);
            await _loanBillRequestRepository.SaveChangesAsync(CancellationToken.None);
        }
        finally
        {
            // ReSharper disable once DisposeOnUsingVariable
            linkedCts.Dispose();
            // ReSharper disable once DisposeOnUsingVariable
            processCts.Dispose();
        }
    }

    public async Task CleanupOldLoanBillDataAsync()
    {
        try
        {
            var cutoffIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(-10);
            var oldLoanBillData = await _loanBillDataRepository
                .FindByCondition(ld => ld.CreatedAt < cutoffIranTime, false)
                .ToListAsync(CancellationToken.None);

            if (oldLoanBillData.Any())
            {
                _loanBillDataRepository.DeleteRange(oldLoanBillData);
                await _loanBillDataRepository.SaveChangesAsync(CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            //_logger.Error($"Error during cleanup of old LoanBillData records | exception message : {ex.Message}");
        }
    }

    private async Task<GetLoanBillDto> CallExternalServiceWithCircuitBreakerAndRetry(string loanId)
    {
        var step = "Start";
        //_logger.Debug($"External service call for LoanId {loanId} - Step: {step}");

        var retryPolicy = Policy
            .Handle<Exception>(ex => !(ex is OperationCanceledException))
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5 * Math.Pow(2, retryAttempt)),
                onRetry: (_, timespan, retryCount, _) =>
                {
                    //_logger.Warn(
                    //    $"Retry {retryCount}/2 for LoanId {loanId} after {timespan.TotalSeconds}s"
                    //);
                });

        var policyWrap = Policy.WrapAsync(_circuitBreakerPolicy, retryPolicy);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        try
        {
            step = "ExecutePolicy";
            //_logger.Debug($"External service call for LoanId {loanId} - Step: {step}");

            return await policyWrap.ExecuteAsync(async (cancellationToken) =>
            {
                step = "CallingExternalService";
                //_logger.Debug($"External service call for LoanId {loanId} - Step: {step} with cancellation token : {cancellationToken}");

                var callStart = DateTime.Now;
                //_logger.Debug($"Making HTTP call to external service for LoanId {loanId} at {callStart} with cancellation token : {cancellationToken}");

                var response = await _loanExternalService.GetLoanBill(
                    new GetLoanBillVm() { LoanId = loanId },
                    cts.Token
                );

                var callEnd = DateTime.Now;

                //_logger.Debug($"External service call for LoanId {loanId} succeeded in {(callEnd - callStart).TotalMilliseconds}ms");

                step = "MappingResponse";
                //_logger.Debug($"External service call for LoanId {loanId} - Step: {step}");

                var mappedDto = _mapper.Map<GetLoanBillDto>(response);
                //_logger.Debug($"Successfully mapped response for LoanId {loanId}");

                return mappedDto;
            }, cts.Token);
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            //_logger.Error($"External service call for LoanId {loanId} TIMED OUT after 60 seconds at step '{step}'");

            throw;
        }
        catch (BrokenCircuitException ex)
        {
            //_logger.Error($"Circuit breaker is OPEN for LoanId {loanId}. Skipping call. | exception message : {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            //_logger.Error($"External service call for LoanId {loanId} FAILED at step '{step}' after retries | exception message : {ex.Message}");
            throw;
        }
    }

    private async Task<GetLoanContractsAllStatementDto> CallExternalServiceWithCircuitBreakerAndRetry_v2(string loanId, int startDate)
    {
        //_logger.Debug($"External service call for LoanId {loanId} - Step: {step}");

        var retryPolicy = Policy
            .Handle<Exception>(ex => !(ex is OperationCanceledException))
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5 * Math.Pow(2, retryAttempt)),
                onRetry: (_, timespan, retryCount, _) =>
                {
                    //_logger.Warn(
                    //    $"Retry {retryCount}/2 for LoanId {loanId} after {timespan.TotalSeconds}s"
                    //);
                });

        var policyWrap = Policy.WrapAsync(_circuitBreakerPolicy, retryPolicy);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        try
        {
            //_logger.Debug($"External service call for LoanId {loanId} - Step: {step}");

            return await policyWrap.ExecuteAsync(async (cancellationToken) =>
            {
                //_logger.Debug($"External service call for LoanId {loanId} - Step: {step} with cancellation token : {cancellationToken}");

                var callStart = DateTime.Now;
                //_logger.Debug($"Making HTTP call to external service for LoanId {loanId} at {callStart} with cancellation token : {cancellationToken}");

                var response = await _loanExternalService.GetLoanContractsAllStatement(
                    new GetLoanContractsAllStatementVm() { LoanId = loanId, StartDate = startDate },
                    cancellationToken
                );

                response.TransactionInformationDtos = response.TransactionInformationDtos
                    .Where(x => !string.IsNullOrWhiteSpace(x.Description1))
                    .ToList();

                var callEnd = DateTime.Now;

                //_logger.Debug($"External service call for LoanId {loanId} succeeded in {(callEnd - callStart).TotalMilliseconds}ms");

                //_logger.Debug($"External service call for LoanId {loanId} - Step: {step}");

                var mappedDto = _mapper.Map<GetLoanContractsAllStatementDto>(response);
                //_logger.Debug($"Successfully mapped response for LoanId {loanId}");

                return mappedDto;
            }, cts.Token);
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            //_logger.Error($"External service call for LoanId {loanId} TIMED OUT after 60 seconds at step '{step}'");

            throw;
        }
        catch (BrokenCircuitException ex)
        {
            //_logger.Error($"Circuit breaker is OPEN for LoanId {loanId}. Skipping call. | exception message : {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            //_logger.Error($"External service call for LoanId {loanId} FAILED at step '{step}' after retries | exception message : {ex.Message}");
            throw;
        }
    }

    private async Task SaveLoanBillData(LoanBillRequest request, GetLoanBillDto loanBill)
    {
        var iranNow = IranTimeHelper.ToIranTimeNow();

        var existing = await _loanBillDataRepository
            .GetByLoanIdAsync(request.LoanId, CancellationToken.None);

        if (existing != null)
        {
            existing.JsonData = JsonConvert.SerializeObject(loanBill);
            existing.UpdatedAt = iranNow;
            await _loanBillDataRepository.UpdateAsync(existing);
        }
        else
        {
            var loanBillData = new LoanBillData
            {
                LoanId = request.LoanId,
                CustomerNo = request.CustomerNo,
                DateUpdate = request.DateUpdate,
                TimeUpdate = request.TimeUpdate,
                JsonData = JsonConvert.SerializeObject(loanBill),
                CreatedAt = iranNow,
                UpdatedAt = iranNow
            };

            await _loanBillDataRepository.CreateAsync(loanBillData);
        }
    }

    private async Task SaveLoanBillData(LoanBillRequest request, GetLoanContractsAllStatementDto loanBill)
    {
        var iranNow = IranTimeHelper.ToIranTimeNow();

        var existing = await _loanBillDataRepository
            .GetByLoanIdAsync(request.LoanId, CancellationToken.None);

        if (existing != null)
        {
            existing.JsonData = JsonConvert.SerializeObject(loanBill);
            existing.UpdatedAt = iranNow;
            await _loanBillDataRepository.UpdateAsync(existing);
        }
        else
        {
            var loanBillData = new LoanBillData
            {
                LoanId = request.LoanId,
                CustomerNo = request.CustomerNo,
                JsonData = JsonConvert.SerializeObject(loanBill),
                CreatedAt = iranNow,
                UpdatedAt = iranNow
            };

            await _loanBillDataRepository.CreateAsync(loanBillData);
        }
    }
}