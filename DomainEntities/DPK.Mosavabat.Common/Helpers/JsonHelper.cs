using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DPK.Mosavabat.Common.Helpers;

public static class JsonHelper
{
    public static JToken ParseJsonString(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON string is null or empty");

        json = json.Trim('\0', '\uFEFF', '\u200B', '\r', '\n', ' ', '\t');

        if (!(json.StartsWith("{") && json.EndsWith("}")))
        {
            throw new JsonReaderException($"JSON does not appear to be an object. Starts with: '{json.Substring(0, Math.Min(10, json.Length))}'");
        }

        return JToken.Parse(json);
    }

    public static string UnescapeJsonString(string? escapedJsonString)
    {
        if (string.IsNullOrEmpty(escapedJsonString))
            return string.Empty;

        var quoted = "\"" + escapedJsonString.Replace("\"", "\\\"").Replace("\\", "\\\\") + "\"";
        var token = JToken.Parse(quoted);
        return token.Value<string>() ?? string.Empty;
    }

    public static List<FlatFieldChange> ExtractFlatFieldChanges(JToken diff)
    {
        var result = new List<FlatFieldChange>();
        TraverseJObject(diff as JObject, [], result);
        return result;
    }

    private static void TraverseJObject(JObject? obj, List<string> currentPath, List<FlatFieldChange> result)
    {
        if (obj == null) return;

        foreach (var prop in obj.Properties())
        {
            if (prop.Name == "_t") continue;

            var newPath = new List<string>(currentPath) { prop.Name };

            switch (prop.Value)
            {
                case JArray { Count: 2 } arr:
                    result.Add(new FlatFieldChange
                    {
                        Path = string.Join(" / ", newPath),
                        OldValue = arr[0].ToString(),
                        NewValue = arr[1].ToString()
                    });
                    break;
                case JObject nestedObj:
                    TraverseJObject(nestedObj, newPath, result);
                    break;
            }
        }
    }
}

public class FlatFieldChange
{
    public string Path { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}

public class DiffResult
{
    public List<SectionChange> Sections { get; set; } = [];
}

public class SectionChange
{
    public string SectionName { get; set; } = string.Empty;
    public List<FieldChange> FieldChanges { get; set; } = [];
    public List<MemberChange> MemberChanges { get; set; } = [];
}

public class FieldChange
{
    public string FieldName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}

public class MemberChange
{
    public string MemberId { get; set; } = string.Empty;
    public List<FieldChange> FieldChanges { get; set; } = [];
}

public class FieldChangeItem
{
    public string Title { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}