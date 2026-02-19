using DPK.Mosavabat.ExternalServices.Bizagi.Errors.UnknownErrorRecorder;
using System.Collections.Concurrent;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Errors.MessageProvider;

public class BizagiMessageProvider
    : IBizagiMessageProvider
{
    private readonly ConcurrentDictionary<string, string> _errors;
    private readonly IBizagiErrorRecorder _errorRecorder;

    public BizagiMessageProvider(IBizagiErrorRecorder errorRecorder)
    {
        _errorRecorder = errorRecorder;
        _errors = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase); // Case-Insensitive
        InitializeErrors();
    }

    private void InitializeErrors()
    {
        // ============================================================
        // 1. خطاهای احراز هویت و دسترسی (Authentication / Authorization)
        // ============================================================
        _errors.TryAdd("CA-100", "شما مجوز دسترسی به این فرآیند یا فعالیت را ندارید.");
        _errors.TryAdd("AUTH_INVALID_USER", "نام کاربری یا کلمه عبور در سیستم فرایندساز نامعتبر است.");
        _errors.TryAdd("AUTH_USER_DISABLED", "حساب کاربری شما در سیستم فرایندساز غیرفعال شده است.");
        _errors.TryAdd("AUTH_DOMAIN_ERROR", "خطا در ارتباط با دامین (Active Directory).");

        // ============================================================
        // 2. خطاهای مربوط به پرونده و فعالیت (Case & Activity)
        // ============================================================
        _errors.TryAdd("CASE_NOT_FOUND", "شماره پرونده (CaseId) یافت نشد.");
        _errors.TryAdd("CASE_CREATION_ERROR", "خطا در ایجاد شماره پرونده جدید.");
        _errors.TryAdd("ACTIVITY_NOT_FOUND", "فعالیت مورد نظر یافت نشد (ممکن است قبلاً انجام شده باشد).");
        _errors.TryAdd("WORKITEM_NOT_FOUND", "آیتم کاری (WorkItem) پیدا نشد.");
        _errors.TryAdd("CASE_LOCKED", "این پرونده در حال حاضر توسط کاربر دیگری در حال ویرایش است.");

        // ============================================================
        // 3. خطاهای ساختاری و داده‌ای (SOA / Data)
        // ============================================================
        _errors.TryAdd("SOA_MISSING_PARAMETER", "برخی پارامترهای اجباری ارسال نشده‌اند.");
        _errors.TryAdd("SOA_INVALID_XML", "ساختار داده ارسالی (XML) معتبر نیست.");
        _errors.TryAdd("XY-500", "خطا در پردازش اطلاعات مشتری (اطلاعات در دیتابیس بیزاجی ناقص است).");
        _errors.TryAdd("ENTITY_NOT_FOUND", "رکورد مورد نظر در موجودیت‌های بیزاجی یافت نشد.");
        _errors.TryAdd("DATA_CONVERSION_ERROR", "خطا در تبدیل نوع داده (مثلاً ارسال متن به جای عدد).");

        // ============================================================
        // 4. خطاهای سیستمی و دیتابیس (System / DB)
        // ============================================================
        _errors.TryAdd("DB_CONNECTION_ERROR", "خطا در برقراری ارتباط با پایگاه داده بیزاجی.");
        _errors.TryAdd("DB_TIMEOUT", "پاسخگویی پایگاه داده بیش از حد طول کشید (Timeout).");
        _errors.TryAdd("SCHEDULER_ERROR", "خطا در زمان‌بندی (Scheduler) سیستم.");
        _errors.TryAdd("INTERNAL_SERVER_ERROR", "خطای داخلی سرور بیزاجی رخ داده است.");

        // ============================================================
        // 5. کدهای خاص (بر اساس تجربیات رایج در پروژه‌های سازمانی)
        // ============================================================
        _errors.TryAdd("P001", "فرآیند مورد نظر معتبر نیست یا نسخه فعالی ندارد.");
        _errors.TryAdd("U001", "کاربر یافت نشد.");
    }

    /// <summary>
    /// دریافت پیام خطا بر اساس کد
    /// </summary>
    public string GetMessage(
        string? errorCode)
    {
        var normalizedCode = errorCode?.Trim();

        if (string.IsNullOrEmpty(
                normalizedCode))
            return "خطای نامشخص از سمت سیستم فرایندساز.";

        if (_errors.TryGetValue(
                normalizedCode,
                out var message))
        {
            return message;
        }

        Task.Run(() => _errorRecorder.RecordUnknownError(normalizedCode));

        return $"خطای سیستمی بیزاجی (کد: {normalizedCode}). لطفاً با پشتیبانی تماس بگیرید.";
    }
}