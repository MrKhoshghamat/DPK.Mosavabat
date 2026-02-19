using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس آیا درخواست از سمت پورتال مشتری حقوقی است
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class IsLcpRequestQuery(
    string mosavabeNo)
    : QueryBase<IsLcpRequestDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}