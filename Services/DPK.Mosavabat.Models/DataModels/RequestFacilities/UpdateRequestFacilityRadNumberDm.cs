namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class UpdateRequestFacilityRadNumberDm(
    int idApprovalRequest,
    string radNumber)
{
    public int IdApprovalRequest { get; } = idApprovalRequest;
    public string RadNumber { get; } = radNumber;
}