namespace DPK.Mosavabat.Services.Contracts.Submission;

public sealed record ReserveSubmissionCommand(
    string SourceSystem,
    string ProcessType,
    long LcpId,
    string? RequestNo,
    byte[] PayloadHash,
    TimeSpan PreparationLease);