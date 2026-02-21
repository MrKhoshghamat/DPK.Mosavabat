using System.Collections.Generic;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.Models.Queries.Lcp;

/// <summary>
/// ورودی هندلر سرویس دریافت لیست اعضای هیئت مدیره
/// </summary>
/// <param name="nationalCode"></param>
public class GetBoardMembersQuery(
    string nationalCode)
    : QueryBase<Result<List<BoardMemberDto>>>
{
    public string NationalCode { get; } = nationalCode;
}