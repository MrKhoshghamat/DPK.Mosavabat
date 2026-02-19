using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetLiveApprovalRequestsAmountsSum;

/// <summary>
/// هندلر سرویس دریافت مجموع مبالغ درخواست مصوبه های در جریان
/// </summary>
internal class GetLiveApprovalRequestsAmountsSumQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetLiveApprovalRequestsAmountsSumQuery,
        GetLiveApprovalRequestsAmountsSumDto>
{
    public async Task<GetLiveApprovalRequestsAmountsSumDto> Handle(
        GetLiveApprovalRequestsAmountsSumQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.GetLiveApprovalRequestsAmountsSum(
            mapper.Map<GetLiveApprovalRequestsAmountsSumDm>(query),
            cancellationToken);
    }
}