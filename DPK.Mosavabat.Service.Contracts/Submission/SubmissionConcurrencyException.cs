namespace DPK.Mosavabat.Services.Contracts.Submission;

public sealed class SubmissionConcurrencyException : Exception
{
    public SubmissionConcurrencyException(
        long submissionId,
        string operation)
        : base(
            $"Submission {submissionId}: operation '{operation}' " +
            "was rejected because the state, ownership, " +
            "or concurrency version changed.")
    {
        SubmissionId = submissionId;
        Operation = operation;
    }

    public long SubmissionId { get; }

    public string Operation { get; }
}