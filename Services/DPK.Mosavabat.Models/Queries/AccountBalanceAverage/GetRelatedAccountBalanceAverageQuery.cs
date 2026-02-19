using System.Collections.Generic;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Queries.AccountBalanceAverage;

/// <summary>
/// ورودی هندلر سرویس دریافت معدل موجودی حساب وابسته
/// </summary>
/// <param name="customerNo"></param>
/// <param name="accountNo"></param>
/// <param name="approvalRequestId"></param>
/// <param name="cuMosavabeId"></param>
/// <param name="typeOfContractLength"></param>
/// <param name="maxContractLength"></param>
/// <param name="code"></param>
/// <param name="currentMonth"></param>
/// <param name="facilityGroup"></param>
public class GetRelatedAccountBalanceAverageQuery(
    [CanBeNull] string customerNo,
    [CanBeNull] string accountNo,
    int approvalRequestId,
    int cuMosavabeId,
    int typeOfContractLength,
    int maxContractLength,
    [CanBeNull] string code,
    [CanBeNull] string currentMonth,
    int facilityGroup)
    : QueryBase<List<AccountBalanceAverageDto>>
{
    [CanBeNull] public string CustomerNo { get; } = customerNo;
    [CanBeNull] public string AccountNo { get; } = accountNo;
    public int ApprovalRequestId { get; } = approvalRequestId;
    public int CuMosavabeId { get; } = cuMosavabeId;
    public int TypeOfContractLength { get; } = typeOfContractLength;
    public int MaxContractLength { get; } = maxContractLength;
    [CanBeNull] public string Code { get; } = code;
    [CanBeNull] public string CurrentMonth { get; } = currentMonth;
    public int FacilityGroup { get; } = facilityGroup;
}