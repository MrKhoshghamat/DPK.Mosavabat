using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace DPK.Mosavabat.Shared.Utilities;

public static class JsonHelper 
{
    public static JToken ParseJsonString(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON string is null or empty");

        // Clean common invisible chars
        json = json.Trim('\0', '\uFEFF', '\u200B', '\r', '\n', ' ', '\t');

        // Ensure it starts and ends with {}
        if (!json.StartsWith("{") || !json.EndsWith("}"))
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
        TraverseJObject(diff as JObject, new List<string>(), result);
        return result;
    }

    private static void TraverseJObject(JObject? obj, List<string> currentPath, List<FlatFieldChange> result)
    {
        if (obj == null) return;

        foreach (var prop in obj.Properties())
        {
            // Skip metadata like "_t"
            if (prop.Name == "_t") continue;

            var newPath = new List<string>(currentPath) { prop.Name };

            if (prop.Value is JArray arr && arr.Count == 2)
            {
                // This is a changed field: [old, new]
                result.Add(new FlatFieldChange
                {
                    Path = string.Join(" / ", newPath),
                    OldValue = arr[0]?.ToString(),
                    NewValue = arr[1]?.ToString()
                });
            }
            else if (prop.Value is JObject nestedObj)
            {
                // Recurse into nested object (e.g., member "2")
                TraverseJObject(nestedObj, newPath, result);
            }
            // Ignore other types (e.g., scalars, arrays not of length 2)
        }
    }
}

public class FlatFieldChange
{
    public string Path { get; set; } = string.Empty; // e.g., "Section / Sub / Field"
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}

public class DiffResult
{
    public List<SectionChange> Sections { get; set; } = new();
}

public class SectionChange
{
    public string SectionName { get; set; } = string.Empty;
    public List<FieldChange> FieldChanges { get; set; } = new();
    public List<MemberChange> MemberChanges { get; set; } = new();
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
    public List<FieldChange> FieldChanges { get; set; } = new();
}

public class FieldChangeItem
{
    public string Title { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
