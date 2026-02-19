using System.Collections.Generic;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس دریافت مفاد و شروط مصوبه
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class GetApprovalRequestTermsAndConditionsQuery(
    string mosavabeNo)
    : QueryBase<List<ApprovalRequestTermsAndConditionsDto>>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}