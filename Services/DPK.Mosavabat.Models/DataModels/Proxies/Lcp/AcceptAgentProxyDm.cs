using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Proxies.Lcp;

public class AcceptAgentProxyDm(
    int agentId,
    int caseId,
    bool isSuccess,
    [CanBeNull] string message)
{
    public int AgentId { get; } = agentId;
    public int CaseId { get; } = caseId;
    public bool IsSuccess { get; } = isSuccess;
    [CanBeNull] public string Message { get; } = message;
}