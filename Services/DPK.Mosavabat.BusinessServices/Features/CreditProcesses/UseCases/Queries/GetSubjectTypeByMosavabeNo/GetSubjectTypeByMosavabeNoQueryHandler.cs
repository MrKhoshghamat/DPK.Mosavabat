using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetSubjectTypeByMosavabeNo;

/// <summary>
/// هندلر سرویس دریافت نوع مصوبه
/// </summary>
internal class GetSubjectTypeByMosavabeNoQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetSubjectTypeByMosavabeNoQuery,
        GetSubjectTypeByMosavabeNoDto>
{
    public async Task<GetSubjectTypeByMosavabeNoDto> Handle(
        GetSubjectTypeByMosavabeNoQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.GetSubjectTypeByMosavabeNo(
            mapper.Map<GetSubjectTypeByMosavabeNoDm>(query),
            cancellationToken);
    }
}