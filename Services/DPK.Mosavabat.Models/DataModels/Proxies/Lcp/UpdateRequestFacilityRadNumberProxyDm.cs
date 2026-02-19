namespace DPK.Mosavabat.Models.DataModels.Proxies.Lcp;

public class UpdateRequestFacilityRadNumberProxyDm(
    int caseId,
    int radNumber,
    int caseIdApprovalRequest)
{
    public int CaseId { get; } = caseId;
    public int RadNumber { get; } = radNumber;
    public int CaseIdApprovalRequest { get; } = caseIdApprovalRequest;
}