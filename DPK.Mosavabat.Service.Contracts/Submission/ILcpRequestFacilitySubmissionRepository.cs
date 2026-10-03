using DPK.Mosavabat.Entities.Models.RequestFacilities;

namespace DPK.Mosavabat.Services.Contracts.Submission;

public interface ILcpRequestFacilitySubmissionRepository
{
    Task<SubmissionReservation> ReserveAsync(
        ReserveSubmissionCommand command,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission?> GetByIdAsync(
        long submissionId,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission?> GetByBusinessKeyAsync(
        string sourceSystem,
        string processType,
        long lcpId,
        CancellationToken cancellationToken = default);

    Task<bool> RenewPreparingLeaseAsync(
        long submissionId,
        Guid ownerToken,
        TimeSpan lease,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission?> TryReacquireBeforeSendAsync(
        long submissionId,
        byte[] expectedRowVersion,
        byte[] expectedPayloadHash,
        TimeSpan lease,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> MarkSendingAsync(
        long submissionId,
        Guid ownerToken,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> MarkRejectedBeforeSendAsync(
        long submissionId,
        Guid ownerToken,
        string? errorCode,
        string? errorMessage,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> MarkUnknownAsync(
        long submissionId,
        Guid ownerToken,
        string? errorCode,
        string? errorMessage,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> RecordBizagiSuccessAsync(
        long submissionId,
        Guid ownerToken,
        int caseId,
        int radNumber,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> RecordReconciledSuccessAsync(
        long submissionId,
        byte[] expectedRowVersion,
        int caseId,
        int radNumber,
        CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> MarkCompletedAsync(
        long submissionId,
        int expectedCaseId,
        int expectedRadNumber,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LcpRequestFacilitySubmission>>
        GetRecoveryCandidatesAsync(
            DateTime olderThanUtc,
            int take,
            CancellationToken cancellationToken = default);

    Task<LcpRequestFacilitySubmission> MarkReconciledFailureAsync(
        long submissionId,
        byte[] expectedRowVersion,
        string? errorMessage,
        CancellationToken cancellationToken = default);
}