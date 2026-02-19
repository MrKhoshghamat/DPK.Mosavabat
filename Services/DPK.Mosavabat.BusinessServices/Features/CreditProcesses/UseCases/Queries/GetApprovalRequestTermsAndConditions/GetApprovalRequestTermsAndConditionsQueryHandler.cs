using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetApprovalRequestTermsAndConditions;

/// <summary>
/// هندلر سرویس دریافت مفاد و شروط مصوبه
/// </summary>
internal class GetApprovalRequestTermsAndConditionsQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetApprovalRequestTermsAndConditionsQuery,
        List<ApprovalRequestTermsAndConditionsDto>>
{
    public async Task<List<ApprovalRequestTermsAndConditionsDto>> Handle(
        GetApprovalRequestTermsAndConditionsQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.GetApprovalRequestTermsAndConditions(
            mapper.Map<GetApprovalRequestTermsAndConditionsDm>(query),
            cancellationToken);
    }
}