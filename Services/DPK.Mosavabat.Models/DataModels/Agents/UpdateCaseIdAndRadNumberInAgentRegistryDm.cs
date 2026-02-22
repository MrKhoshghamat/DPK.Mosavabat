namespace DPK.Mosavabat.Models.DataModels.Agents;

public class UpdateCaseIdAndRadNumberInAgentRegistryDm(
    AgentDm agentDm,
    int caseId,
    int radNumber)
{
    public AgentDm AgentDm { get; } = agentDm;
    public int CaseId { get; } = caseId;
    public int RadNumber { get; } = radNumber;
}