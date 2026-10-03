using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Entities.Models.RequestFacilities;
using DPK.Mosavabat.Services.Contracts.Submission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DPK.Mosavabat.Services.Jobs;

public sealed class SubmissionReconciliationHostedService : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(30);

    private static readonly TimeSpan GracePeriodBeforeDefinitiveFailure = TimeSpan.FromMinutes(15);

    private const int BatchSize = 50;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILoggerManager _logger;

    public SubmissionReconciliationHostedService(
        IServiceScopeFactory scopeFactory, ILoggerManager logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ReconcileBatchAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.Error($"Reconciliation loop failed: {ex}");
            }

            await Task.Delay(PollInterval, stoppingToken);
        }
    }

    private async Task ReconcileBatchAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ILcpRequestFacilitySubmissionRepository>();
        var requestFacilityDatabase = scope.ServiceProvider.GetRequiredService<IRequestFacilityDatabase>();

        var candidates = await repo.GetRecoveryCandidatesAsync(DateTime.UtcNow, BatchSize, ct);

        foreach (var submission in candidates)
        {
            try
            {
                await ReconcileOneAsync(submission, repo, requestFacilityDatabase, ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.Trace($"Submission {submission.Id} changed concurrently during reconciliation; skipping this cycle.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to reconcile submission {submission.Id}: {ex}");
            }
        }
    }

    private async Task ReconcileOneAsync(
        LcpRequestFacilitySubmission submission,
        ILcpRequestFacilitySubmissionRepository repo,
        IRequestFacilityDatabase requestFacilityDatabase,
        CancellationToken ct)
    {
        var actual = await requestFacilityDatabase.GetLcpRequestFacilityByGuidAsync(
            submission.CorrelationId.ToString(), ct);

        if (actual is { CaseId: > 0 })
        {
            await repo.RecordReconciledSuccessAsync(
                submission.Id, submission.RowVersion,
                actual.CaseId!.Value, actual.RadNumber ?? 0, ct);

            _logger.Trace(
                $"Reconciled submission {submission.Id}: found CaseId={actual.CaseId}, RadNumber={actual.RadNumber}");
            return;
        }

        var staleFor = DateTime.UtcNow - submission.UpdatedAt;
        if (staleFor < GracePeriodBeforeDefinitiveFailure)
        {
            _logger.Trace(
                $"Submission {submission.Id} still within grace period " +
                $"({staleFor.TotalMinutes:F1}m of {GracePeriodBeforeDefinitiveFailure.TotalMinutes:F0}m); leaving as {submission.Status}.");
            return;
        }

        await repo.MarkReconciledFailureAsync(
            submission.Id, submission.RowVersion,
            $"No matching case found after {GracePeriodBeforeDefinitiveFailure.TotalMinutes:F0} minutes of waiting; marked as failed so a new client submission can retry.",
            ct);

        _logger.Trace(
            $"Submission {submission.Id} marked RejectedBeforeSend after grace period — eligible for retry on next client submission.");
    }
}