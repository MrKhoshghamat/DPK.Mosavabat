namespace DPK.Mosavabat.Models.DataModels.Proxies.Lcp;

public class CreateTicketProxyDm(
    bool hasSubjectFacilityId,
    int subjectFacilityId,
    string cuApprovalSubjectId,
    string title,
    string description,
    string requestFacilityStatus,
    long number,
    string taskId)
{
    public bool HasSubjectFacilityId { get; } = hasSubjectFacilityId;
    public int SubjectFacilityId { get; } = subjectFacilityId;
    public string CuApprovalSubjectId { get; } = cuApprovalSubjectId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public string RequestFacilityStatus { get; } = requestFacilityStatus;
    public long Number { get; } = number;
    public string TaskId { get; } = taskId;
}