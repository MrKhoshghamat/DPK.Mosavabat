using DPK.Mosavabat.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DPK.Mosavabat.Services.Jobs;

public class ProcessLoanBillJob : IJob
{
    private static bool _hasLoggedNoRequests;
    private readonly ILoanBillRequestRepository _loanBillRequestRepository;
    //private readonly ILoggerManager _loggerManager;
    private readonly IServiceProvider _serviceProvider;

    public ProcessLoanBillJob(
        ILoanBillRequestRepository loanBillRequestRepository,
        IServiceProvider serviceProvider
        /*ILoggerManager loggerManager*/)
    {
        _loanBillRequestRepository = loanBillRequestRepository;
        _serviceProvider = serviceProvider;
        //_loggerManager = loggerManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<ProcessLoanBillJobScopedService>();

        await scopedService.CleanupOldLoanBillDataAsync();

        var pendingRequests = await _loanBillRequestRepository
            .GetPendingRequestsAsync(50, CancellationToken.None);

        if (!pendingRequests.Any())
        {
            if (_hasLoggedNoRequests) return;
            //_loggerManager.Trace("No pending requests found.");
            _hasLoggedNoRequests = true;
            return;
        }

        _hasLoggedNoRequests = false;

        var maxConcurrency = Math.Min(5, Math.Max(1, pendingRequests.Count / 4));
        //_loggerManager.Trace($"Processing {pendingRequests.Count} requests with max {maxConcurrency} concurrent operations.");

        var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

        var tasks = pendingRequests.Select(async request =>
        {
            await semaphore.WaitAsync();
            try
            {
                using var taskScope = _serviceProvider.CreateScope();
                var taskScopedService = taskScope.ServiceProvider.GetRequiredService<ProcessLoanBillJobScopedService>();
                await taskScopedService.ProcessSingleLoanBillAsync(request);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
}