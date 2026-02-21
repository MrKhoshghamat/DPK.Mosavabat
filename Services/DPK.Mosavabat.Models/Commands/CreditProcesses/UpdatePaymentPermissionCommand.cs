using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.Models.Commands.CreditProcesses;

/// <summary>
/// ورودی هندلر سرویس بروز رسانی مجوز پرداخت | حذف یا ابطال
/// </summary>
/// <param name="id"> شناسه مجوز پرداخت </param>
/// <param name="isDeleted"> حذف شده </param>
/// <param name="isCanceled"> ابطال شده </param>
public class UpdatePaymentPermissionCommand(
    int id,
    bool isDeleted,
    bool isCanceled)
    : CommandBase<Result<UpdatePaymentPermissionDto>>
{
    /// <summary>
    /// شناسه مجوز پرداخت
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    /// حذف شده
    /// </summary>
    public bool IsDeleted { get; } = isDeleted;

    /// <summary>
    /// ابطال شده
    /// </summary>
    public bool IsCanceled { get; } = isCanceled;
}