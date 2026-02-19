using Refit;

namespace DPK.Mosavabat.ExternalServices.Utilities;

public class CamelCaseUrlParameterKeyFormatter : IUrlParameterKeyFormatter
{
    public string Format(string key)
    {
        if (string.IsNullOrEmpty(key) || char.IsLower(key[0]))
            return key;

        return char.ToLowerInvariant(key[0]) + key[1..];
    }
}