using System.Collections.Generic;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

namespace DPK.Mosavabat.Models.Queries.ApprovalRequests;

/// <summary>
/// ورودی هندلر سرویس دریافت وضعیت درخواست
/// </summary>
/// <param name="idCase"></param>
public class GetApprovalRequestStatusQuery(
    int idCase)
    : QueryBase<Result<List<ApprovalRequestStatusDto>>>
{
    public int IdCase { get; } = idCase;
}