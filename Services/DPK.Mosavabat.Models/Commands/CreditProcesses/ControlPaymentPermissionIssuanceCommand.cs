using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس کنترل صدور مجوز پرداخت
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
/// <param name="contractAmount"> مبلغ قرارداد </param>
/// <param name="customerSharePrice"> مبلغ پیش دریافت مشتری</param>
/// <param name="subContractType"> زیر نوع عقد </param>
/// <param name="customerNo"> شماره مشتری </param>
public class ControlPaymentPermissionIssuanceCommand(
    string mosavabeNo,
    long contractAmount,
    long? customerSharePrice,
    int? subContractType,
    string customerNo)
    : CommandBase<ControlPaymentPermissionIssuanceDto>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;

    /// <summary>
    /// مبلغ قرارداد
    /// </summary>
    public long ContractAmount { get; } = contractAmount;

    /// <summary>
    /// مبلغ پیش دریافت مشتری
    /// </summary>
    public long? CustomerSharePrice { get; } = customerSharePrice;

    /// <summary>
    /// زیر نوع عقد
    /// </summary>
    public int? SubContractType { get; } = subContractType;

    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;
}