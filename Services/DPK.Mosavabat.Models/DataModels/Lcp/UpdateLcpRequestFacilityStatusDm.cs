using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Lcp;

public class UpdateLcpRequestFacilityStatusDm(
    int caseId,
    [CanBeNull] string requestFacilityStatus,
    [CanBeNull] string message)
{
    public int CaseId { get; } = caseId;
    [CanBeNull] public string RequestFacilityStatus { get; } = requestFacilityStatus;
    [CanBeNull] public string Message { get; } = message;
}