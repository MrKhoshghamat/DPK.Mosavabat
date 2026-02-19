using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Errors.UnknownErrorRecorder;

public class FileBizagiErrorRecorder(
    ILogger<FileBizagiErrorRecorder> logger)
    : IBizagiErrorRecorder
{
    private readonly string _filePath = Path
        .Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "BizagiUnknownErrors.log");

    // استفاده از ConcurrentDictionary به عنوان یک کش سریع (Thread-Safe)
    // تا از نوشتن تکراری یک کد خطای ناشناخته در فایل جلوگیری کنیم.
    private static readonly ConcurrentDictionary<string, bool> RecordedCodes = new();
    private readonly object _fileLock = new();

    // فایل در کنار فایل اجرایی برنامه ساخته می‌شود

    public void RecordUnknownError(string errorCode)
    {
        if (string.IsNullOrWhiteSpace(errorCode)) return;

        // اگر این کد قبلاً در این سشن ثبت شده است، بیخیال شو (جهت پرفورمنس)
        if (RecordedCodes.ContainsKey(errorCode)) return;

        try
        {
            // تلاش برای افزودن به کش مموری
            if (!RecordedCodes.TryAdd(
                    errorCode, true)) return;
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | NEW_CODE: {errorCode} | NEEDS_TRANSLATION";

            // قفل کردن فایل برای جلوگیری از خطای همزمانی هنگام نوشتن
            lock (_fileLock)
            {
                File.AppendAllText(
                    _filePath,
                    logMessage + Environment.NewLine,
                    Encoding.UTF8);
            }

            // لاگ کردن در کنسول/دیتابیس لاگ‌ها هم انجام شود
            logger.LogWarning(
                "New Bizagi Error Code Detected and Recorded: {ErrorCode}",
                errorCode);
        }
        catch (Exception ex)
        {
            // نباید خطای رکورد کردن باعث توقف برنامه اصلی شود
            logger.LogError(
                ex,
                "Failed to record unknown Bizagi error code.");
        }
    }
}