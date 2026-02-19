using System.Collections.Generic;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس دریافت مجوزهای پرداخت مصوبه
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class GetPaymentPermissionsQuery(
    string mosavabeNo)
    : QueryBase<List<PaymentPermissionsDto>>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}