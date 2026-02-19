using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس دریافت مجموع مبالغ درخواست مصوبه های در جریان
/// </summary>
/// <param name="customerNo"> شماره مشتری </param>
/// <param name="idApprovalRequest"> شناسه درخواست </param>
public class GetLiveApprovalRequestsAmountsSumQuery(
    string customerNo,
    int idApprovalRequest)
    : QueryBase<GetLiveApprovalRequestsAmountsSumDto>
{
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;

    /// <summary>
    /// شناسه درخواست
    /// </summary>
    public int IdApprovalRequest { get; } = idApprovalRequest;
}