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

namespace DPK.Mosavabat.BusinessServices.Features.Contracts.UseCases.Queries.GetSubContractTypeTitleBySubContractTypeId;

/// <summary>
/// هندلر سرویس دریافت عنوان زیر نوع قرارداد
/// </summary>
internal class GetSubContractTypeTitleBySubContractTypeIdQueryHandler(
    IContractSqlService contractSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetSubContractTypeTitleBySubContractTypeIdQuery,
        Result<GetSubContractTypeTitleBySubContractTypeIdDto>>
{
    public async Task<Result<GetSubContractTypeTitleBySubContractTypeIdDto>> Handle(
        GetSubContractTypeTitleBySubContractTypeIdQuery query,
        CancellationToken cancellationToken)
    {
        return await Result.Success(query)
            .Map(mapper.Map<GetSubContractTypeTitleBySubContractTypeIdDm>)
            .Bind(dm => contractSqlService.GetSubContractTypeTitleBySubContractTypeId(
                dm, cancellationToken));
    }
}