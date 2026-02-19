using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.Lcp;

public class AcceptAgentVm
{
    public int AgentId { get; set; }
    public int CaseId { get; set; }
    public bool IsSuccess { get; set; }
    [CanBeNull] public string Message { get; set; }
}