namespace DPK.Mosavabat.Common.Extensions;

public static class CollectionExtension
{
    public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }
}