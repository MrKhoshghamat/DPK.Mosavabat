using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.Contracts;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;
using DPK.Mosavabat.Models.Queries.Contracts;
using DPK.Mosavabat.SqlBusinessServices.Contracts.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.Contracts.UseCases.Queries.GetApprovalContracts;

/// <summary>
/// هندلر سرویس دریافت قراردادهای مصوبه
/// </summary>
internal class GetApprovalContractsQueryHandler(
    IContractSqlService contractSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetApprovalContractsQuery,
        Result<List<ContractDto>>>
{
    public async Task<Result<List<ContractDto>>> Handle(
        GetApprovalContractsQuery query,
        CancellationToken cancellationToken)
    {
        return await Result.Success(query)
            .Map(mapper.Map<GetApprovalContractsDm>)
            .Bind(dm => contractSqlService.GetApprovalContracts(
                dm,
                cancellationToken));
    }
}