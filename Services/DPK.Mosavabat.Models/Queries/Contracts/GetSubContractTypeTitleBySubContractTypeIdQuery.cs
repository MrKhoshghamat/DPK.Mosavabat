using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;

namespace DPK.Mosavabat.Models.Queries.Contracts;

public class GetSubContractTypeTitleBySubContractTypeIdQuery(
    int subContractTypeId)
    : QueryBase<Result<GetSubContractTypeTitleBySubContractTypeIdDto>>
{
    public int SubContractTypeId { get; } = subContractTypeId;
}