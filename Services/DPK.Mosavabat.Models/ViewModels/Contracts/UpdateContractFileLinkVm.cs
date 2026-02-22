using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.Contracts;

public class UpdateContractFileLinkVm
{
    [CanBeNull] public string FileLink { get; set; }
    [CanBeNull] public string CuApprovalSubjectId { get; set; }
}