namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class UpdateRequestFacilityStatusDm(
    int caseId)
{
    public int CaseId { get; } = caseId;
}