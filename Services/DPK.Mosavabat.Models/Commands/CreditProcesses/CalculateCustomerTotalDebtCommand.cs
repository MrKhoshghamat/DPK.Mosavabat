using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس محاسبه مانده بدهی کل مشتری
/// </summary>
/// <param name="customerNo"> شماره مشتری </param>
/// <param name="receivedDailyPaymentAmountsSum"> مجموع مبالغ وصولی روز </param>
public class CalculateCustomerTotalDebtCommand(
    string customerNo,
    long receivedDailyPaymentAmountsSum)
    : CommandBase<CalculateCustomerTotalDebtDto>
{
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;

    /// <summary>
    /// مجموع مبالغ وصولی روز
    /// </summary>
    public long ReceivedDailyPaymentAmountsSum { get; } = receivedDailyPaymentAmountsSum;
}