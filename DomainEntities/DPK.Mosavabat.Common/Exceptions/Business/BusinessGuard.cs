using System.Collections.Generic;

namespace DPK.Mosavabat.Common.Exceptions.Business;

public static class BusinessGuard
{
    public static void ThrowIfNull<T>(
        T? input,
        BusinessError error,
        IDictionary<string, object?>? metadata = null)
    {
        if (input is null)
            throw BusinessRuleException.FromSingle(
                error,
                metadata);
    }

    public static void ThrowIf(
        bool condition,
        BusinessError error,
        IDictionary<string, object?>? metadata = null)
    {
        if (condition)
            throw BusinessRuleException.FromSingle(error, metadata);
    }
}