using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس دریافت نوع مصوبه
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class GetSubjectTypeByMosavabeNoQuery(
    string mosavabeNo)
    : QueryBase<GetSubjectTypeByMosavabeNoDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}