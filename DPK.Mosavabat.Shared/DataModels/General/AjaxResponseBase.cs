using System.Text.Json;

namespace DPK.Mosavabat.Shared.DataModels.General;

public class AjaxResponseBase<T>
{
    public T? Result { get; set; }

    /// <summary>
    ///     Indicates success status of the result.
    ///     Set <see cref="Error" /> if this value is false.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    ///     Error details (Must and only set if <see cref="Success" /> is false).
    /// </summary>
    public ErrorInfo? Error { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
[Serializable]
public class ErrorInfo
{
    /// <summary>
    ///     Error code.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    ///     Error message.
    /// </summary>
    public string? Message { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}