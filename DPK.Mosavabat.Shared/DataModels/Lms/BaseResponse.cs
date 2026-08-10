using DPK.Mosavabat.Shared.DataModels.General;

namespace DPK.Mosavabat.Shared.DataModels.Lms;

public class BaseResponse<T> where T : class
{
    public T Result { get; set; }
    public bool IsAuthenticate { get; set; }
    public bool HasException { get; set; }
    public bool Success { get; set; }
    public ErrorInfo? Error { get; set; }
}