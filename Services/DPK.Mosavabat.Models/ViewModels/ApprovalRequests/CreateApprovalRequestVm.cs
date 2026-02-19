using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.ApprovalRequests;

public class CreateApprovalRequestVm
{
    public int LcpRequestFacilityIdCase { get; set; }
    public int LcpRequestFacilityRadNumber { get; set; }
    public int LcpRequestFacilityId { get; set; }
    [CanBeNull] public string GuidLcpRequestFacility { get; set; }
}