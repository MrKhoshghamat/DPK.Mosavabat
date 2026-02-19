using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس جمع پرداختی های روز مشتری
/// </summary>
/// <param name="customerNo"> شماره مشتری </param>
public class AggregateDailyCustomerPaymentCommand(
    string customerNo)
    : CommandBase<AggregateDailyCustomerPaymentDto>
{
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;
}