namespace DPK.Mosavabat.Common.Idempotency;

/// <summary>
/// یک درخواست با همین کلید کسب‌وکاری (SourceSystem+ProcessType+LcpId) در حال حاضر
/// در وضعیت Preparing/Sending/BizagiCreated است و هنوز به نتیجه‌ی نهایی نرسیده.
/// کلاینت نباید Retry کور کور بزنه؛ باید صبر کنه یا بعداً وضعیت رو استعلام بگیره.
/// </summary>
public sealed class SubmissionInProgressException : Exception
{
    public long SubmissionId { get; }

    public SubmissionInProgressException(long submissionId)
        : base($"Submission {submissionId} is already in progress; a duplicate request was suppressed.")
    {
        SubmissionId = submissionId;
    }
}

/// <summary>
/// پاسخ فراخوانی قبلی به Bizagi (Transport Failure/Timeout) گم شده و مشخص نیست
/// Case واقعاً ساخته شده یا نه. تا زمان Reconciliation نباید تلاش جدیدی انجام بشه.
/// </summary>
public sealed class SubmissionPendingReconciliationException : Exception
{
    public long SubmissionId { get; }

    public SubmissionPendingReconciliationException(long submissionId)
        : base($"Submission {submissionId} has an unknown outcome and is pending reconciliation.")
    {
        SubmissionId = submissionId;
    }
}

/// <summary>
/// درخواستی با همین کلید کسب‌وکاری قبلاً با محتوای متفاوت ثبت شده است —
/// این یعنی یا یک باگ در تولید LcpId/RequestNo سمت کلاینت هست، یا یک تلاش عمدی
/// برای بازنویسی یک درخواست تکمیل‌شده با داده‌ی دیگر.
/// </summary>
public sealed class SubmissionPayloadMismatchException : Exception
{
    public long ExistingSubmissionId { get; }

    public SubmissionPayloadMismatchException(long existingSubmissionId)
        : base($"A submission with the same business key already exists (Id={existingSubmissionId}) " +
               "with a different payload.")
    {
        ExistingSubmissionId = existingSubmissionId;
    }
}