using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس دریافت اطلاعات درخواست مصوبه
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class GetApprovalRequestInfoQuery(
    string mosavabeNo)
    : QueryBase<GetApprovalRequestInfoDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}