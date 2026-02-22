using System.Collections.Generic;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;

namespace DPK.Mosavabat.Models.Queries.Contracts;

public class GetApprovalContractsQuery(
    string mosavabeNo)
    : QueryBase<Result<List<ContractDto>>>
{
    public string MosavabeNo { get; } = mosavabeNo;
}