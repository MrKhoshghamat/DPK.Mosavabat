using DPK.Mosavabat.Common.Idempotency;
using DPK.Mosavabat.Entities.Models.RequestFacilities;
using DPK.Mosavabat.Services.Contracts.Submission;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace DPK.Mosavabat.Repository;

public sealed class LcpRequestFacilitySubmissionRepository(RepositoryContext db)
    : ILcpRequestFacilitySubmissionRepository
{
    public async Task<SubmissionReservation> ReserveAsync(
        ReserveSubmissionCommand command, CancellationToken ct = default)
    {
        var existing = await GetByBusinessKeyAsync(
            command.SourceSystem, command.ProcessType, command.LcpId, ct);

        if (existing is null)
        {
            var entity = new LcpRequestFacilitySubmission
            {
                SourceSystem = command.SourceSystem,
                ProcessType = command.ProcessType,
                LcpId = command.LcpId,
                RequestNo = command.RequestNo,
                CorrelationId = Guid.NewGuid(),
                PayloadHash = command.PayloadHash,
                Status = SubmissionStatus.Preparing,
                OwnerToken = Guid.NewGuid(),
                LeaseUntil = DateTime.Now.Add(command.PreparationLease),
                AttemptCount = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            db.Set<LcpRequestFacilitySubmission>().Add(entity);

            try
            {
                await db.SaveChangesAsync(ct);
                return new SubmissionReservation(ReservationOutcome.Acquired, entity);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                db.Entry(entity).State = EntityState.Detached;
                existing = await GetByBusinessKeyAsync(
                               command.SourceSystem, command.ProcessType, command.LcpId, ct)
                           ?? throw new InvalidOperationException(
                               "Race در Reserve شناسایی شد ولی رکورد یافت نشد.");
            }
        }

        if (existing.Status == SubmissionStatus.RejectedBeforeSend)
            return await TryReclaimAsync(existing, command, allowPayloadChange: true, ct);

        var leaseExpired = existing.LeaseUntil is null || existing.LeaseUntil < DateTime.Now;
        if (existing.Status != SubmissionStatus.Preparing || !leaseExpired)
            return !PayloadEquals(existing.PayloadHash, command.PayloadHash)
                ? new SubmissionReservation(ReservationOutcome.PayloadMismatch, existing)
                : new SubmissionReservation(ReservationOutcome.Existing, existing);
        if (!PayloadEquals(existing.PayloadHash, command.PayloadHash))
            return new SubmissionReservation(ReservationOutcome.PayloadMismatch, existing);

        return await TryReclaimAsync(existing, command, allowPayloadChange: false, ct);

    }

    private async Task<SubmissionReservation> TryReclaimAsync(
        LcpRequestFacilitySubmission existing,
        ReserveSubmissionCommand command,
        bool allowPayloadChange,
        CancellationToken ct)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
            .FirstOrDefaultAsync(x => x.Id == existing.Id, ct);

        if (tracked is null)
            return new SubmissionReservation(ReservationOutcome.Existing, existing);

        if (!tracked.RowVersion.AsSpan().SequenceEqual(existing.RowVersion))
            return new SubmissionReservation(ReservationOutcome.Existing, tracked);

        var now = DateTime.Now;
        var ownerToken = Guid.NewGuid();

        tracked.Status = SubmissionStatus.Preparing;
        tracked.OwnerToken = ownerToken;
        tracked.LeaseUntil = now.Add(command.PreparationLease);
        tracked.AttemptCount = existing.AttemptCount + 1;
        tracked.UpdatedAt = now;
        if (allowPayloadChange)
            tracked.PayloadHash = command.PayloadHash;

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            DetachTracked(tracked);
            var latest = await GetByIdAsync(existing.Id, ct);
            return new SubmissionReservation(ReservationOutcome.Existing, latest!);
        }

        return new SubmissionReservation(ReservationOutcome.Acquired, tracked);
    }

    public Task<LcpRequestFacilitySubmission?> GetByIdAsync(long id, CancellationToken ct = default) =>
        db.Set<LcpRequestFacilitySubmission>().AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<LcpRequestFacilitySubmission?> GetByBusinessKeyAsync(
        string sourceSystem, string processType, long lcpId, CancellationToken ct = default) =>
        db.Set<LcpRequestFacilitySubmission>().AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.SourceSystem == sourceSystem &&
                x.ProcessType == processType &&
                x.LcpId == lcpId, ct);

    public async Task<LcpRequestFacilitySubmission?> TryReacquireBeforeSendAsync(
        long submissionId, byte[] expectedRowVersion, byte[] expectedPayloadHash,
        TimeSpan lease, CancellationToken ct = default)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
            .FirstOrDefaultAsync(x => x.Id == submissionId, ct);

        if (tracked is null || tracked.Status != SubmissionStatus.Preparing)
            return null;

        if (!tracked.RowVersion.AsSpan().SequenceEqual(expectedRowVersion) ||
            !tracked.PayloadHash.AsSpan().SequenceEqual(expectedPayloadHash))
            return null;

        tracked.LeaseUntil = DateTime.Now.Add(lease);
        tracked.UpdatedAt = DateTime.Now;

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            DetachTracked(tracked);
            return null;
        }

        return tracked;
    }

    public async Task<LcpRequestFacilitySubmission> MarkSendingAsync(
        long submissionId, Guid ownerToken, CancellationToken ct = default)
    {
        var tracked = await RequireOwnedAsync(submissionId, ownerToken, ct);
        tracked.Status = SubmissionStatus.Sending;
        tracked.SendingAt = DateTime.Now;
        tracked.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync(ct);
        return tracked;
    }

    public async Task<LcpRequestFacilitySubmission> MarkRejectedBeforeSendAsync(
        long submissionId, Guid ownerToken, string? errorCode, string? errorMessage,
        CancellationToken ct = default)
    {
        var tracked = await RequireOwnedAsync(submissionId, ownerToken, ct);
        tracked.Status = SubmissionStatus.RejectedBeforeSend;
        tracked.LastErrorCode = errorCode;
        tracked.LastErrorMessage = errorMessage;
        tracked.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync(ct);
        return tracked;
    }

    public async Task<LcpRequestFacilitySubmission> MarkUnknownAsync(
        long submissionId, Guid ownerToken, string? errorCode, string? errorMessage,
        CancellationToken ct = default)
    {
        var tracked = await RequireOwnedAsync(submissionId, ownerToken, ct);
        tracked.Status = SubmissionStatus.Unknown;
        tracked.LastErrorCode = errorCode;
        tracked.LastErrorMessage = errorMessage;
        tracked.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync(ct);
        return tracked;
    }

    public async Task<LcpRequestFacilitySubmission> RecordBizagiSuccessAsync(
        long submissionId, Guid ownerToken, int caseId, int radNumber, CancellationToken ct = default)
    {
        var tracked = await RequireOwnedAsync(submissionId, ownerToken, ct);
        tracked.Status = SubmissionStatus.BizagiCreated;
        tracked.CaseId = caseId;
        tracked.RadNumber = radNumber;
        tracked.BizagiCreatedAt = DateTime.Now;
        tracked.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync(ct);
        return tracked;
    }

    public async Task<LcpRequestFacilitySubmission> RecordReconciledSuccessAsync(
        long submissionId, byte[] expectedRowVersion, int caseId, int radNumber, CancellationToken ct = default)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
                          .FirstOrDefaultAsync(x => x.Id == submissionId, ct)
                      ?? throw new InvalidOperationException($"Submission {submissionId} not found.");

        if (!tracked.RowVersion.AsSpan().SequenceEqual(expectedRowVersion))
            throw new DbUpdateConcurrencyException(
                $"Submission {submissionId} was modified concurrently during reconciliation.");

        tracked.Status = SubmissionStatus.BizagiCreated;
        tracked.CaseId = caseId;
        tracked.RadNumber = radNumber;
        tracked.BizagiCreatedAt = DateTime.Now;
        tracked.LastErrorMessage = "Reconciled after transport failure";
        tracked.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync(ct);
        return tracked;
    }

    public async Task<LcpRequestFacilitySubmission> MarkCompletedAsync(
        long submissionId, int expectedCaseId, int expectedRadNumber, CancellationToken ct = default)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
                          .FirstOrDefaultAsync(x => x.Id == submissionId, ct)
                      ?? throw new InvalidOperationException($"Submission {submissionId} not found.");

        if (tracked.CaseId != expectedCaseId || tracked.RadNumber != expectedRadNumber)
            throw new InvalidOperationException(
                $"CaseId/RadNumber mismatch on complete for submission {submissionId}.");

        tracked.Status = SubmissionStatus.Completed;
        tracked.CompletedAt = DateTime.Now;
        tracked.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync(ct);
        return tracked;
    }

    public async Task<bool> RenewPreparingLeaseAsync(
        long submissionId, Guid ownerToken, TimeSpan lease, CancellationToken ct = default)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
            .FirstOrDefaultAsync(x => x.Id == submissionId, ct);

        if (tracked is null || tracked.OwnerToken != ownerToken || tracked.Status != SubmissionStatus.Preparing)
            return false;

        tracked.LeaseUntil = DateTime.Now.Add(lease);
        tracked.UpdatedAt = DateTime.Now;

        try
        {
            await db.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            DetachTracked(tracked);
            return false;
        }
    }

    public Task<IReadOnlyList<LcpRequestFacilitySubmission>> GetRecoveryCandidatesAsync(
        DateTime olderThanUtc, int take, CancellationToken ct = default) =>
        db.Set<LcpRequestFacilitySubmission>().AsNoTracking()
            .Where(x =>
                (x.Status == SubmissionStatus.Sending || x.Status == SubmissionStatus.Unknown)
                && (x.LeaseUntil == null || x.LeaseUntil < olderThanUtc))
            .OrderBy(x => x.UpdatedAt)
            .Take(take)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<LcpRequestFacilitySubmission>)t.Result, ct);

    public async Task<LcpRequestFacilitySubmission> MarkReconciledFailureAsync(
        long submissionId, byte[] expectedRowVersion, string? errorMessage, CancellationToken ct = default)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
                          .FirstOrDefaultAsync(x => x.Id == submissionId, ct)
                      ?? throw new InvalidOperationException($"Submission {submissionId} not found.");

        if (!tracked.RowVersion.AsSpan().SequenceEqual(expectedRowVersion))
            throw new DbUpdateConcurrencyException(
                $"Submission {submissionId} was modified concurrently during reconciliation.");

        tracked.Status = SubmissionStatus.RejectedBeforeSend;
        tracked.LastErrorMessage = errorMessage;
        tracked.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return tracked;
    }

    private async Task<LcpRequestFacilitySubmission> RequireOwnedAsync(
        long submissionId, Guid ownerToken, CancellationToken ct)
    {
        var tracked = await db.Set<LcpRequestFacilitySubmission>()
                          .FirstOrDefaultAsync(x => x.Id == submissionId, ct)
                      ?? throw new InvalidOperationException($"Submission {submissionId} not found.");

        if (tracked.OwnerToken != ownerToken)
            throw new InvalidOperationException(
                $"Ownership mismatch for submission {submissionId}: caller no longer owns the lease.");

        return tracked;
    }

    private void DetachTracked(LcpRequestFacilitySubmission entity) =>
        db.Entry(entity).State = EntityState.Detached;

    private static bool PayloadEquals(byte[] a, byte[] b) => a.AsSpan().SequenceEqual(b);

    private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
        ex.InnerException is SqlException { Number: 2601 or 2627 };
}