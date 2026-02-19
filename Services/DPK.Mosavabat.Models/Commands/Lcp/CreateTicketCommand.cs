using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.Models.Commands.Lcp;

/// <summary>
/// ورودی هندلر سرویس درخواست ثبت تیکت
/// </summary>
/// <param name="hasSubjectFacilityId"></param>
/// <param name="subjectFacilityId"></param>
/// <param name="cuApprovalSubjectId"></param>
/// <param name="title"></param>
/// <param name="description"></param>
/// <param name="requestFacilityStatus"></param>
/// <param name="number"></param>
/// <param name="taskId"></param>
/// <param name="requestFacilityId"></param>
public class CreateTicketCommand(
    bool hasSubjectFacilityId,
    int subjectFacilityId,
    string cuApprovalSubjectId,
    string title,
    string description,
    string requestFacilityStatus,
    long number,
    string taskId,
    int requestFacilityId)
    : CommandBase<CreateTicketDto>
{
    public bool HasSubjectFacilityId { get; } = hasSubjectFacilityId;
    public int SubjectFacilityId { get; } = subjectFacilityId;
    public string CuApprovalSubjectId { get; } = cuApprovalSubjectId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public string RequestFacilityStatus { get; } = requestFacilityStatus;
    public long Number { get; } = number;
    public string TaskId { get; } = taskId;
    public int RequestFacilityId { get; } = requestFacilityId;
}