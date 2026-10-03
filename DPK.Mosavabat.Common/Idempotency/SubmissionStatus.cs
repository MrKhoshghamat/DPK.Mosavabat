namespace DPK.Mosavabat.Common.Idempotency;

public enum SubmissionStatus : byte
{
    Preparing = 1,
    Sending = 2,
    BizagiCreated = 3,
    Completed = 4,
    RejectedBeforeSend = 5,
    Unknown = 6
}