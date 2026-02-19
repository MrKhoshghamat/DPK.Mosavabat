using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetApprovalRequestInfo;

/// <summary>
/// هندلر سرویس دریافت اطلاعات درخواست مصوبه
/// </summary>
/// <param name="creditProcessSqlService"></param>
/// <param name="mapper"></param>
internal class GetApprovalRequestInfoQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetApprovalRequestInfoQuery,
        GetApprovalRequestInfoDto>
{
    public async Task<GetApprovalRequestInfoDto> Handle(
        GetApprovalRequestInfoQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.GetApprovalRequestInfo(
            mapper.Map<GetApprovalRequestInfoDm>(
                query),
            cancellationToken);
    }
}