using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.Queries.ApprovalRequests;
using DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.UseCases.Queries.GetRequestsLastStatus;

/// <summary>
/// هندلر سرویس دریافت آخرین وضعیت درخواست ها
/// </summary>
internal class GetRequestsLastStatusQueryHandler(
    IApprovalRequestSqlService approvalRequestSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetApprovalRequestsLastStatusQuery,
        Result<GetApprovalRequestsLastStatusesDto>>
{
    public async Task<Result<GetApprovalRequestsLastStatusesDto>> Handle(
        GetApprovalRequestsLastStatusQuery query,
        CancellationToken cancellationToken)
    {
        return await approvalRequestSqlService
            .GetRequestsLastStatuses(
                mapper.Map<GetApprovalRequestsLastStatusDm>(query),
                cancellationToken)
            .Map(items => new GetApprovalRequestsLastStatusesDto(
                items.ToDictionary(
                    r => r.CaseId,
                    r => r.StatusDescription)
            ));
    }
}