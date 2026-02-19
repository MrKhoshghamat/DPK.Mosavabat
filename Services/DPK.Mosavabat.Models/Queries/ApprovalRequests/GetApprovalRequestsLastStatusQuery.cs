using System.Collections.Generic;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

namespace DPK.Mosavabat.Models.Queries.ApprovalRequests;

/// <summary>
/// ورودی هندلر سرویس دریافت آخرین وضعیت درخواست ها
/// </summary>
/// <param name="caseIds"></param>
public class GetApprovalRequestsLastStatusQuery(
    List<int> caseIds)
    : QueryBase<Result<GetApprovalRequestsLastStatusesDto>>
{
    public List<int> CaseIds { get; } = caseIds;
}