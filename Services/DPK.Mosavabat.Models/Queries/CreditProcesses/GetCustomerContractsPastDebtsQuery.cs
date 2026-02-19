using System.Collections.Generic;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Queries.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس دریافت بدهی های گذشته مشتری
/// </summary>
/// <param name="customerNo"> شماره مشتری </param>
public class GetCustomerContractsPastDebtsQuery(
    string customerNo)
    : QueryBase<List<CustomerPastDebtDto>>
{
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;
}