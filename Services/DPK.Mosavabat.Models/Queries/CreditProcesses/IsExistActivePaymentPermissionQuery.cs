using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس موجودی مجوز پرداخت فعال
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class IsExistActivePaymentPermissionQuery(
    string mosavabeNo)
    : QueryBase<IsExistActivePaymentPermissionDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}