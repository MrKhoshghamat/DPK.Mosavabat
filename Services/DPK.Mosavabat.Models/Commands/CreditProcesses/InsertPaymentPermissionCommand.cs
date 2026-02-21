using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس ایجاد شناسه پرداخت
/// </summary>
/// <param name="mosavabeNo"> شماره مصوبه </param>
/// <param name="branchCode"> کد شعبه </param>
/// <param name="customerNo"> شماره مشتری </param>
/// <param name="contractType"> نوع عقد </param>
/// <param name="subContractType"> زیرنوع عقد </param>
/// <param name="contractAmount"> مبلغ قرارداد </param>
/// <param name="prepaidAmount"> مبلغ پیش دریافت </param>
public class InsertPaymentPermissionCommand(
    string mosavabeNo,
    string branchCode,
    string customerNo,
    int contractType,
    int? subContractType,
    long contractAmount,
    long prepaidAmount)
    : CommandBase<Result<InsertPaymentPermissionDto>>
{
    /// <summary>
    /// شماره مصوبه
    /// </summary>
    public string MosavabeNo { get; } = mosavabeNo;

    /// <summary>
    /// کد شعبه
    /// </summary>
    public string BranchCode { get; } = branchCode;

    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerNo { get; } = customerNo;

    /// <summary>
    /// نوع عقد
    /// </summary>
    public int ContractType { get; } = contractType;

    /// <summary>
    /// زیر نوع عقد
    /// </summary>
    public int? SubContractType { get; } = subContractType;

    /// <summary>
    /// مبلغ قرارداد
    /// </summary>
    public long ContractAmount { get; } = contractAmount;

    /// <summary>
    /// مبلغ پیش دریافت
    /// </summary>
    public long PrepaidAmount { get; } = prepaidAmount;
}