using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Foundation.Dependency;
using DPK.Foundation.Domain.Uow;
using DPK.Mosavabat.Common.Helpers;
using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using DPK.Mosavabat.DomainEntities.LoanBill.Repositories;
using DPK.Mosavabat.ExternalServices.Abrisham.Loan.Contract;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.LoanBills;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;

namespace DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Service;

public class ProcessLoanBillJobScopedService 
    : ITransientDependency
{
    private readonly ILoanBillRequestRepository _loanBillRequestRepository;
    private readonly ILoanBillDataRepository _loanBillDataRepository;
    private readonly ILoanExternalService _loanExternalService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProcessLoanBillJobScopedService> _logger;
    private readonly IUnitOfWorkManager _uow;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    public ProcessLoanBillJobScopedService(
        ILoanBillRequestRepository loanBillRequestRepository,
        ILoanBillDataRepository loanBillDataRepository,
        ILoanExternalService loanExternalService,
        IMapper mapper,
        IUnitOfWorkManager uow,
        ILogger<ProcessLoanBillJobScopedService> logger)
    {
        _loanBillRequestRepository = loanBillRequestRepository;
        _loanBillDataRepository = loanBillDataRepository;
        _loanExternalService = loanExternalService;
        _mapper = mapper;
        _logger = logger;
        _uow = uow;

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (ex, _) => _logger.LogWarning($"Circuit breaker opened due to {ex.Message}"),
                onReset: () => _logger.LogWarning("Circuit breaker reset"),
                onHalfOpen: () => _logger.LogWarning("Circuit breaker half-open - next call is a trial"));
    }

    public async Task ProcessSingleLoanBillAsync(LoanBillRequest request)
    {
        const string step = "Started";
        // Timeout for single request processing
        using var processCts = new CancellationTokenSource(TimeSpan.FromMinutes(2));

        try
        {
            using (var uow = _uow.Begin(TransactionScopeOption.RequiresNew))
            {
                request.IsProcessing = true;
                await _loanBillRequestRepository.UpdateAsync(request, processCts.Token);
                await uow.CompleteAsync();
            }

            var loanBill = await CallExternalServiceWithCircuitBreakerAndRetry(request.LoanId, processCts.Token);

            using (var uow = _uow.Begin(TransactionScopeOption.RequiresNew))
            {
                await SaveLoanBillData(request, loanBill, processCts.Token);

                request.IsCompleted = true;
                request.IsProcessing = false;
                request.CompletedAt = IranTimeHelper.ToIranTimeNow();
                await _loanBillRequestRepository.UpdateAsync(request, processCts.Token);

                await uow.CompleteAsync();
            }

            _logger.LogInformation($"Successfully completed processing for LoanId {request.LoanId}");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(request, ex, step);
        }
    }

    private async Task HandleExceptionAsync(LoanBillRequest request, Exception ex, string step)
    {
        _logger.LogError(ex, $"Failed to process LoanId {request.LoanId} at step '{step}'");

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
            request.IsCompleted = true;
            request.IsProcessing = false;
        }

        try
        {
            using var uow = _uow.Begin(TransactionScopeOption.RequiresNew);
            await _loanBillRequestRepository.UpdateAsync(request, CancellationToken.None);
            await uow.CompleteAsync();
        }
        catch (Exception updateEx)
        {
            _logger.LogError(updateEx, "Failed to update error status for request.");
        }
    }

    public async Task CleanupOldLoanBillDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var cutoffIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(-10);

            using var uow = _uow.Begin(TransactionScopeOption.RequiresNew);

            var oldLoanBillData = await _loanBillDataRepository
                .GetOldLoanBillsAsync(cutoffIranTime, cancellationToken);

            if (oldLoanBillData != null && oldLoanBillData.Count != 0)
            {
                await _loanBillDataRepository.DeleteRangeAsync(oldLoanBillData, cancellationToken);
                await uow.CompleteAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup of old LoanBillData records");
        }
    }

    private async Task<GetLoanBillDto> CallExternalServiceWithCircuitBreakerAndRetry(string loanId, CancellationToken parentToken)
    {
        var retryPolicy = Policy
            .Handle<Exception>(ex => ex is not OperationCanceledException)
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5 * Math.Pow(2, retryAttempt)));

        var policyWrap = Policy.WrapAsync(_circuitBreakerPolicy, retryPolicy);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(parentToken);
        cts.CancelAfter(TimeSpan.FromSeconds(60));

        return await policyWrap.ExecuteAsync(async (ct) =>
        {
            var response = await _loanExternalService.GetLoanBill(
                new GetLoanBillDm(loanId),
                ct
            );
            return _mapper.Map<GetLoanBillDto>(response);
        }, cts.Token);
    }

    private async Task SaveLoanBillData(LoanBillRequest request, GetLoanBillDto loanBill, CancellationToken cancellationToken)
    {
        var iranNow = IranTimeHelper.ToIranTimeNow();
        var existing = await _loanBillDataRepository.GetByLoanIdAsync(request.LoanId, cancellationToken);

        if (existing != null)
        {
            existing.JsonData = JsonConvert.SerializeObject(loanBill);
            existing.UpdatedAt = iranNow;
            await _loanBillDataRepository.UpdateAsync(existing, cancellationToken);
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
            await _loanBillDataRepository.CreateAsync(loanBillData, cancellationToken);
        }
    }
}
