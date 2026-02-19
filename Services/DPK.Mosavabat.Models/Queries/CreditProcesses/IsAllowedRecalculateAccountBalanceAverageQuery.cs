using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس مجاز به محاسبه مجدد معدل موجودی
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
/// <param name="contractAmount"> مبلغ قرارداد </param>
public class IsAllowedRecalculateAccountBalanceAverageQuery(
    string mosavabeNo,
    long contractAmount)
    : QueryBase<IsAllowedRecalculateAccountBalanceAverageDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;

    /// <summary>
    /// مبلغ قرارداد
    /// </summary>
    public long ContractAmount { get; } = contractAmount;
}