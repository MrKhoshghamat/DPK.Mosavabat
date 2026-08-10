using System.Net;

namespace DPK.Mosavabat.Shared.Lcp.Models.General;

public class LcpResponse<T>
{
    /// <summary>
    ///     نتیجه بازگشتی
    /// </summary>
    public T? Result { get; set; }

    /// <summary>
    ///     عملیات موفق بوده
    ///     به عبارت دیگر 200 برگشته
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    ///     کد
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; set; }

    /// <summary>
    ///     در صورتی که عملیات ناموفق باشد
    ///     خطا در اینجا قرار می گیرد
    /// </summary>
    public string? ExceptionMessage { get; set; }
}