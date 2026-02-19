namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class GetUserNameByCaseIdDm(
    int caseId)
{
    public int CaseId { get; } = caseId;
}