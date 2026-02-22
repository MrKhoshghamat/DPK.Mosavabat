using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;

namespace DPK.Mosavabat.Models.Queries.Contracts;

public class GetContractTypeTitleByContractTypeIdQuery(
    int contractTypeId)
    : QueryBase<Result<GetContractTypeTitleByContractTypeIdDto>>
{
    public int ContractTypeId { get; } = contractTypeId;
}