using DPK.Mosavabat.Common.Idempotency;

namespace DPK.Mosavabat.Entities.Models.RequestFacilities;

public class LcpRequestFacilitySubmission
{
    public long Id { get; set; }

    public string SourceSystem { get; set; } = string.Empty;

    public string ProcessType { get; set; } = string.Empty;

    public long LcpId { get; set; }

    public string? RequestNo { get; set; }

    public Guid CorrelationId { get; set; }

    public byte[] PayloadHash { get; set; } = Array.Empty<byte>();

    public SubmissionStatus Status { get; set; }

    public Guid OwnerToken { get; set; }

    public DateTime? LeaseUntil { get; set; }

    public int AttemptCount { get; set; }

    public int? CaseId { get; set; }

    public int? RadNumber { get; set; }

    public string? LastErrorCode { get; set; }

    public string? LastErrorMessage { get; set; }

    // All database timestamps are UTC.
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SendingAt { get; set; }

    public DateTime? BizagiCreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}