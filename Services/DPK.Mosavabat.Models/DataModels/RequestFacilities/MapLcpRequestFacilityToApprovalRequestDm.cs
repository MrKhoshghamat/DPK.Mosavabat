namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class MapLcpRequestFacilityToApprovalRequestDm(
    int idLcpRequestFacility,
    int idCase,
    int idCaseApprovalRequest)
{
    public int IdLcpRequestFacility { get; } = idLcpRequestFacility;
    public int IdCase { get; } = idCase;
    public int IdCaseApprovalRequest { get; } = idCaseApprovalRequest;
}