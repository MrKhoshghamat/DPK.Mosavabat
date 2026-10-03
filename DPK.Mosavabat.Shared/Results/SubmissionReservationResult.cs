using DPK.Mosavabat.Common.Idempotency;

namespace DPK.Mosavabat.Shared.Results;

public sealed class SubmissionReservationResult
{
    public bool IsOwner { get; init; }

    public bool PayloadMismatch { get; init; }

    public SubmissionStatus Status { get; init; }

    public Guid CorrelationId { get; init; }

    public int? CaseId { get; init; }

    public int? RadNumber { get; init; }

    public string? ErrorMessage { get; init; }
}
