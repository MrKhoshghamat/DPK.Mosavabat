using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.IsExistActivePaymentPermission;

/// <summary>
/// هندلر سرویس موجودی مجوز پرداخت فعال
/// </summary>
internal class IsExistActivePaymentPermissionQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : IQueryHandler<
        IsExistActivePaymentPermissionQuery,
        IsExistActivePaymentPermissionDto>
{
    public async Task<IsExistActivePaymentPermissionDto> Handle(
        IsExistActivePaymentPermissionQuery query,
        CancellationToken cancellationToken)
    {
        return await creditProcessSqlService.IsExistActivePaymentPermission(
            mapper.Map<IsExistActivePaymentPermissionDm>(query),
            cancellationToken);
    }
}