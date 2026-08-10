using DPK.Mosavabat.Shared.DataModels.General;

namespace DPK.Mosavabat.Shared.DataModels.Lcp;

public class LcpBaseResponse<T> where T : class
{
    public T Result { get; set; }
    public bool IsAuthenticate { get; set; }
    public bool HasException { get; set; }
    public bool Success { get; set; }
    public ErrorInfo? Error { get; set; }
}