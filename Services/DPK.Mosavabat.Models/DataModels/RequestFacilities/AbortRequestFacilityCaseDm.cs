namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class AbortRequestFacilityCaseDm(
    int caseId)
{
    public int CaseId { get; } = caseId;
}