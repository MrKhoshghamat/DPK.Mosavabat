using DPK.Mosavabat.Common.Exceptions;

namespace DPK.Mosavabat.Common.Extensions;

public static class StringExtension
{
    public static Stack<string> Tokens { get; }
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    static StringExtension()
    {
        Tokens = new Stack<string>();
    }

    private static readonly Random Random = new();
    
    public static string RandomString()
    {
        return CreateRandomString(32);
    }

    public static string CreateRandomCodeString(int length)
    {
        var code = new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
        return code;
    }
    public static string CreateRandomString(int length)
    {
        var code = CreateRandomCodeString(length - 5);
        return $"{new Random().Next(10000, 99999)}{code}";
    }

    public static decimal ToDecimal(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return 0;
        if (!decimal.TryParse(input, out var result))
            throw new DpkException("دیتای ورودی نامعتبر می باشد");
        return result;
    }

    public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

    public static TSelf TrimStringProperties<TSelf>(this TSelf input)
    {
        var stringProperties = input?.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

        if (stringProperties == null) return input;
        foreach (var stringProperty in stringProperties)
        {
            var currentValue = (string)stringProperty.GetValue(input, null)!;
            stringProperty.SetValue(input, currentValue.Trim(), null);
        }

        return input;
    }
}