using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.Queries.ApprovalRequests;
using DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.UseCases.Queries.GetApprovalRequestStatuses;

/// <summary>
/// هندلر سرویس دریافت وضعیت های درخواست
/// </summary>
internal class GetApprovalRequestStatusesQueryHandler(
    IApprovalRequestSqlService approvalRequestSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetApprovalRequestStatusQuery,
        Result<List<ApprovalRequestStatusDto>>>
{
    public async Task<Result<List<ApprovalRequestStatusDto>>> Handle(
        GetApprovalRequestStatusQuery query,
        CancellationToken cancellationToken)
    {
        return await approvalRequestSqlService.GetApprovalRequestStatuses(
            mapper.Map<GetApprovalRequestStatusesDm>(query),
            cancellationToken);
    }
}