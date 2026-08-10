namespace DPK.Mosavabat.Shared.Lcp.Models.General;

public class LcpApiResponse
{
    public object? Result { get; set; }
    public string? TargetUrl { get; set; }
    public bool Success { get; set; }
    public LcpErrorInfo? Error { get; set; }
    public bool UnAuthorizedRequest { get; set; }
    public bool __dpk { get; set; }
    public string? ReferenceId { get; set; }
}
