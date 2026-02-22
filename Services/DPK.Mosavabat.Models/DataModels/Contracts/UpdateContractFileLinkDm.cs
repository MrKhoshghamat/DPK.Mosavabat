using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Contracts;

public class UpdateContractFileLinkDm(
    [CanBeNull] string fileLink,
    [CanBeNull] string cuApprovalSubjectId)
{
    [CanBeNull] public string FileLink { get; } = fileLink;
    [CanBeNull] public string CuApprovalSubjectId { get; } = cuApprovalSubjectId;
}