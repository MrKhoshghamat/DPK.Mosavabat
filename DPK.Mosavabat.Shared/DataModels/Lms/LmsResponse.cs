using DPK.Mosavabat.Shared.Dictionaries.CreditProcesses;
using DPK.Mosavabat.Shared.Enums.CreditProcesses;

namespace DPK.Mosavabat.Shared.DataModels.Lms;

public class LmsResponse
{
    public Errors? Error { get; set; }
    public string? Result { get; set; }
}