using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.Lcp;

/// <summary>
/// ورودی هندلر سرویس تائید نماینده
/// </summary>
/// <param name="agentId"></param>
/// <param name="caseId"></param>
/// <param name="isSuccess"></param>
/// <param name="message"></param>
public class AcceptAgentCommand(
    int agentId,
    int caseId,
    bool isSuccess,
    [CanBeNull] string message)
    : CommandBase<AcceptAgentDto>
{
    public int AgentId { get; } = agentId;
    public int CaseId { get; } = caseId;
    public bool IsSuccess { get; } = isSuccess;
    [CanBeNull] public string Message { get; } = message;
}