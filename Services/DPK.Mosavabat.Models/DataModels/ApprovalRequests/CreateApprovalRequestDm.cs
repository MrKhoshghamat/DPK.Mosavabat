using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.ApprovalRequests;

public class CreateApprovalRequestDm(
    int lcpRequestFacilityIdCase,
    int lcpRequestFacilityRadNumber,
    int lcpRequestFacilityId,
    [CanBeNull] string guidLcpRequestFacility)
{
    public int LcpRequestFacilityIdCase { get; } = lcpRequestFacilityIdCase;
    public int LcpRequestFacilityRadNumber { get; } = lcpRequestFacilityRadNumber;
    public int LcpRequestFacilityId { get; } = lcpRequestFacilityId;
    [CanBeNull] public string GuidLcpRequestFacility { get; } = guidLcpRequestFacility;
}