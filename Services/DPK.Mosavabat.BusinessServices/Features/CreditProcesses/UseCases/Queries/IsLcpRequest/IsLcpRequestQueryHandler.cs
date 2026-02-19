using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.IsLcpRequest;

/// <summary>
/// هندلر سرویس آیا درخواست از سمت پورتال مشتریان حقوقی است
/// </summary>
internal class IsLcpRequestQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        IsLcpRequestQuery,
        IsLcpRequestDto>
{
    public async Task<IsLcpRequestDto> Handle(
        IsLcpRequestQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.IsLcpRequest(
            mapper.Map<IsLcpRequestDm>(query),
            cancellationToken);
    }
}