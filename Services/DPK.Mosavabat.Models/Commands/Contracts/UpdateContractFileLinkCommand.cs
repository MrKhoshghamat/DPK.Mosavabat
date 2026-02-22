using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.Contracts;

public class UpdateContractFileLinkCommand(
    [CanBeNull] string fileLink,
    [CanBeNull] string cuApprovalSubjectId)
    : CommandBase<Result<UpdateContractFileLinkDto>>
{
    [CanBeNull] public string FileLink { get; } = fileLink;
    [CanBeNull] public string CuApprovalSubjectId { get; } = cuApprovalSubjectId;
}