using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Foundation.Quartz;
using DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Service;
using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using DPK.Mosavabat.DomainEntities.LoanBill.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DPK.Mosavabat.BusinessServices.Jobs.LoanBill.Executor;

/// <summary>
/// جاب پردازش درخواست‌های استعلام وام
/// </summary>
public class ProcessLoanBillJob : JobBase, ITransientDependency
{
    private static bool _hasLoggedNoRequests;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProcessLoanBillJob> _logger;

    /// <summary>
    /// سازنده کلاس
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    public ProcessLoanBillJob(
        IServiceProvider serviceProvider,
        ILogger<ProcessLoanBillJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// اجرای جاب
    /// </summary>
    /// <param name="context"></param>
    public override async Task Execute(IJobExecutionContext context)
    {
        var jobKey = context.JobDetail.Key;
        _logger.LogInformation($"Executing Job: {jobKey} at {DateTime.Now}");

        // 1. Cleanup Old Data (in a separate scope)
        using (var scope = _serviceProvider.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<ProcessLoanBillJobScopedService>();
            await scopedService.CleanupOldLoanBillDataAsync();
        }

        List<LoanBillRequest> pendingRequests;

        // 2. Fetch Pending Requests (in a separate scope to prevent DbContext disposal issues)
        using (var scope = _serviceProvider.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ILoanBillRequestRepository>();
            pendingRequests = await repo.GetPendingRequestsAsync(50, CancellationToken.None);
        }

        if (pendingRequests == null || pendingRequests.Count == 0)
        {
            if (_hasLoggedNoRequests) return;
            _logger.LogTrace("No pending requests found.");
            _hasLoggedNoRequests = true;
            return;
        }

        _hasLoggedNoRequests = false;

        // Dynamic concurrency limit based on load
        var maxConcurrency = Math.Min(5, Math.Max(1, pendingRequests.Count / 4));
        _logger.LogInformation($"Processing {pendingRequests.Count} requests with max {maxConcurrency} concurrent operations.");

        var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

        // 3. Parallel Processing
        var tasks = pendingRequests.Select(async request =>
        {
            await semaphore.WaitAsync();
            try
            {
                using var taskScope = _serviceProvider.CreateScope();
                var taskScopedService = taskScope.ServiceProvider.GetRequiredService<ProcessLoanBillJobScopedService>();

                await taskScopedService.ProcessSingleLoanBillAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Critical error in parallel processing loop for request {request.Id}");
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
}
