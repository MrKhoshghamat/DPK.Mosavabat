using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس ساخت فرایند تکمیل اطلاعات قرارداد
/// </summary>
/// <param name="mabAsl"> مبلغ اصل</param>
/// <param name="cuApprovalSubjectId"> شناسه موضوع مصوبه | شماره مصوبه </param>
/// <param name="c10"> شماره مشتری </param>
/// <param name="c13"> شماره 13 رقمی تسهیلات سپهری </param>
public class CreateCompleteContractInfoProcessCommand(
    decimal? mabAsl,
    int cuApprovalSubjectId,
    [CanBeNull] string c10,
    [CanBeNull] string c13)
    : CommandBase<Result<CreateCompleteContractInfoProcessDto>>
{
    /// <summary>
    /// مبلغ اصل
    /// </summary>
    public decimal? MabAsl { get; } = mabAsl;

    /// <summary>
    /// شناسه موضوع مصوبه | شماره مصوبه
    /// </summary>
    public int CuApprovalSubjectId { get; } = cuApprovalSubjectId;

    /// <summary>
    /// شماره مشتری
    /// </summary>
    [CanBeNull]
    public string C10 { get; } = c10;

    /// <summary>
    /// شماره 13 رقمی تسهیلات سپهری
    /// </summary>
    [CanBeNull]
    public string C13 { get; } = c13;
}