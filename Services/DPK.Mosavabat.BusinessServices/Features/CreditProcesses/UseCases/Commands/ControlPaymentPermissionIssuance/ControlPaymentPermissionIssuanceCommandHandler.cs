using System.Collections.Generic;
using System.Linq;
using DPK.Foundation;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.ControlPaymentPermissionIssuance;

/// <summary>
/// خطاهای بیزینسی
/// </summary>
public sealed class BusinessRuleException : DpkException
{
    /// <summary>
    /// کد
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// پیام خطا
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// کلید خطا
    /// </summary>
    public string? ErrorKey { get; }

    /// <summary>
    /// لیست خطاها
    /// </summary>
    public IReadOnlyList<BusinessError> Errors { get; }

    /// <summary>
    /// اطلاعات تکمیلی
    /// </summary>
    public IReadOnlyDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// سازنده کلاس
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="errorKey"></param>
    /// <param name="metadata"></param>
    /// <param name="innerException"></param>
    public BusinessRuleException(
        int code,
        string message,
        string? errorKey = null,
        IDictionary<string, object?>? metadata = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        ErrorMessage = message;
        ErrorKey = errorKey;
        Errors = [new BusinessError(code, message, errorKey)];
        Metadata = NormalizeMetadata(metadata);
        AttachMetadataToExceptionData(Metadata);
    }

    /// <summary>
    /// سازنده کلاس
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="message"></param>
    /// <param name="metadata"></param>
    /// <param name="innerException"></param>
    public BusinessRuleException(
        IEnumerable<BusinessError> errors,
        string? message = null,
        IDictionary<string, object?>? metadata = null,
        Exception? innerException = null)
        : base(message ?? "خطای بیزینسی رخ داده است.", innerException)
    {
        var list = errors.ToList();
        if (list.Count == 0)
        {
            list.Add(new BusinessError(-1, message ?? "خطای بیزینسی نامشخص"));
        }

        Errors = list;
        Code = list[0].Code;
        ErrorMessage = message ?? list[0].Message;
        ErrorKey = list[0].ErrorKey;
        Metadata = NormalizeMetadata(metadata);
        AttachMetadataToExceptionData(Metadata);
    }

    /// <summary>
    /// تکی
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static BusinessRuleException Single(int code, string message, string? key = null)
        => new(code, message, key);

    /// <summary>
    /// چندتایی
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static BusinessRuleException Multiple(IEnumerable<BusinessError> errors, string? message = null)
        => new(errors, message);

    /// <summary>
    /// تفکیک اطلاعات تکمیلی
    /// </summary>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public static IReadOnlyDictionary<string, object?> NormalizeMetadata(
        IDictionary<string, object?>? metadata)
        => metadata is null
            ? []
            : new Dictionary<string, object?>(metadata);

    private void AttachMetadataToExceptionData(IReadOnlyDictionary<string, object?> metadata)
    {
        foreach (var kv in metadata)
        {
            if (!Data.Contains(kv.Key))
                Data[kv.Key] = kv.Value;
        }
    }
}

/// <summary>
/// خطاهای بیزینسی
/// </summary>
/// <param name="Code"></param>
/// <param name="Message"></param>
/// <param name="ErrorKey"></param>
/// <param name="Target"></param>
public record BusinessError(
    int Code,
    string Message,
    string? ErrorKey = null,
    string? Target = null
);
