namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class GetApprovalRequestIdDm(
    int caseId)
{
    public int CaseId { get; } = caseId;
}