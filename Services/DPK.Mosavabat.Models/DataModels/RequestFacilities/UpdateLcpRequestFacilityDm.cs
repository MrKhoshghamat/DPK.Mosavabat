using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class UpdateLcpRequestFacilityDm(
    int idCase,
    int initialRadNumber,
    int idCaseApprovalRequest,
    int radNumberApprovalRequest,
    int lcpRequestFacilityId,
    [CanBeNull] string guidLcpRequestFacility)
{
    public int IdCase { get; set; }
    public int InitialRadNumber { get; set; }
    public int? IdCaseApprovalRequest { get; set; }
    public int? RadNumberApprovalRequest { get; set; }
    public int? LcpRequestFacilityId { get; set; }
    public string? GuidLcpRequestFacility { get; set; }
}