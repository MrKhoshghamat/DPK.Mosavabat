using System.Collections.Generic;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندل سرویس دریافت قراردادهای پرداخت شده به مشتری در روز
/// </summary>
/// <param name="customerNo"> شماره مشتری </param>
public class GetDailyPaidContractsQuery(
    string customerNo)
    : QueryBase<List<CustomerDailyPaidContractDto>>
{
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;
}