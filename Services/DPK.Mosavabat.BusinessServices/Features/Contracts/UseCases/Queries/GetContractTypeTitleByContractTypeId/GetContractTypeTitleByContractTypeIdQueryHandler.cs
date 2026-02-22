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

namespace DPK.Mosavabat.BusinessServices.Features.Contracts.UseCases.Queries.GetContractTypeTitleByContractTypeId;

/// <summary>
/// هندلر سرویس دریافت عنوان نوع قرارداد
/// </summary>
internal class GetContractTypeTitleByContractTypeIdQueryHandler(
    IContractSqlService contractSqlService,
    IMapper mapper)
    : IQueryHandler<
        GetContractTypeTitleByContractTypeIdQuery,
        Result<GetContractTypeTitleByContractTypeIdDto>>
{
    public async Task<Result<GetContractTypeTitleByContractTypeIdDto>> Handle(
        GetContractTypeTitleByContractTypeIdQuery query,
        CancellationToken cancellationToken)
    {
        return await Result.Success(query)
            .Map(mapper.Map<GetContractTypeTitleByContractTypeIdDm>)
            .Bind(dm => contractSqlService.GetContractTypeTitleByContractTypeId(
                dm, cancellationToken));
    }
}