using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.Lcp;

public class UpdateRequestFacilityStatusVm
{
    public int CaseId { get; set; }
    [CanBeNull] public string RequestFacilityStatus { get; set; }
    [CanBeNull] public string Message { get; set; }
}