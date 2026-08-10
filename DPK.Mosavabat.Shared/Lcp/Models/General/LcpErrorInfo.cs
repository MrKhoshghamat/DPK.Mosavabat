namespace DPK.Mosavabat.Shared.Lcp.Models.General;

public class LcpErrorInfo
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public object? Details { get; set; }
    public object? ValidationErrors { get; set; }
}
