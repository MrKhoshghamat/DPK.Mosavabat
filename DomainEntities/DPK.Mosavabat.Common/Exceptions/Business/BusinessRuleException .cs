using System.Collections.Generic;
using System.Linq;

namespace DPK.Mosavabat.Common.Exceptions.Business;

public record BusinessError(
    int Code,
    string Message,
    string? Key = null,
    string? Target = null);

public sealed class BusinessRuleException
    : Exception
{
    public IReadOnlyList<BusinessError> Errors { get; }
    public IReadOnlyDictionary<string, object?> Metadata { get; }

    public BusinessRuleException(
        IEnumerable<BusinessError> errors,
        string? message = null,
        IDictionary<string, object?>? metadata = null,
        Exception? innerException = null)
        : base(message ?? errors.FirstOrDefault()?.Message ?? "Business rule violated.", innerException)
    {
        Errors = errors.ToArray();
        Metadata = metadata == null
            ? []
            : new Dictionary<string, object?>(metadata);

        foreach (var (key, value) in Metadata)
        {
            if (!Data.Contains(key))
                Data.Add(key, value);
        }
    }

    public static BusinessRuleException FromSingle(
        BusinessError error,
        IDictionary<string, object?>? metadata = null)
        => new([error], metadata: metadata);
}