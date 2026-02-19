using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Proxies.Lcp;

public class UpdateLcpRequestFacilityStatusProxyDm(
    int caseId,
    [CanBeNull] string requestFacilityStatus,
    [CanBeNull] string message)
{
    public int CaseId { get; } = caseId;
    [CanBeNull] public string RequestFacilityStatus { get; } = requestFacilityStatus;
    [CanBeNull] public string Message { get; } = message;
}