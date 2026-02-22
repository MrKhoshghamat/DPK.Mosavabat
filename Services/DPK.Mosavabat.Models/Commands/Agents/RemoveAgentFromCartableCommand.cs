using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Agents;

namespace DPK.Mosavabat.Models.Commands.Agents;

public class RemoveAgentFromCartableCommand(
    int caseId)
    : CommandBase<Result<RemoveAgentFromCartableDto>>
{
    public int CaseId { get; } = caseId;
}