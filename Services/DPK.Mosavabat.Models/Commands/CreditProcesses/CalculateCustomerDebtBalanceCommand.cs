using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس محاسبه مانده بدهی مشتری
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class CalculateCustomerDebtBalanceCommand(
    string mosavabeNo)
    : CommandBase<CalculateCustomerDebtBalanceDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}