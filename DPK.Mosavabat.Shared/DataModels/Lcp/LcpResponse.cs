using DPK.Mosavabat.Shared.Dictionaries.CreditProcesses;

namespace DPK.Mosavabat.Shared.DataModels.Lcp;

public class LcpResponse
{
    public Errors? Error { get; set; }
    public string? Result { get; set; }
}