using System.Collections.Generic;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی سرویس دریافت قراردادها و مجوزهای پرداخت مصوبه
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class GetContractsQuery(
    string mosavabeNo)
    : QueryBase<List<ContractDto>>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}