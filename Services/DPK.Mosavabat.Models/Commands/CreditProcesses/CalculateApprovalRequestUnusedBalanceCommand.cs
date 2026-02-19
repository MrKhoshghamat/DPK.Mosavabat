using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس محاسبه مانده آزاد مصوبه
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
public class CalculateApprovalRequestUnusedBalanceCommand(
    string mosavabeNo)
    : CommandBase<CalculateApprovalRequestUnusedBalanceDto>
{
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;
}