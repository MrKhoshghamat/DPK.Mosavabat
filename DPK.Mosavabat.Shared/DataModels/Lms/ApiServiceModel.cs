namespace DPK.Mosavabat.Shared.DataModels.Lms;

public sealed record ApiServiceModel(string Result, bool IsAuthorized)
{
    public static ApiServiceModel IsUnAuthorize => new(null, false);
    public static ApiServiceModel IsAuthorize(string result) => new(result, true);
}