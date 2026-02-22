namespace DPK.Mosavabat.Models.DataModels.Agents;

public class GetUserNameByCaseIdDm(
    int caseId)
{
    public int CaseId { get; } = caseId;
}