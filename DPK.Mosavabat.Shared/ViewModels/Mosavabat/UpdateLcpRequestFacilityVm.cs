namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

public class UpdateLcpRequestFacilityVm
{
    public int IdCase { get; set; }
    public int InitialRadNumber { get; set; }
    public int? IdCaseApprovalRequest { get; set; }
    public int? RadNumberApprovalRequest { get; set; }
    public int? LcpRequestFacilityId { get; set; }
    public string? GuidLcpRequestFacility { get; set; }
}