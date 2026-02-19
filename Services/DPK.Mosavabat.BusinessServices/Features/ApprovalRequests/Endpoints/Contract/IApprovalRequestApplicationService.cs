using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.ViewModels.ApprovalRequests;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Endpoints.Contract;

/// <summary>
/// سرویس های درخواست صدور مصوبه
/// </summary>
public interface IApprovalRequestApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس ایجاد درخواست صدور مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateApprovalRequestDto> CreateApprovalRequest(
        CreateApprovalRequestVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت وضعیت های درخواست
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ApprovalRequestStatusDto>> GetApprovalRequestStatuses(
        GetApprovalRequestStatusesVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت آخرین وضعیت درخواست ها
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetApprovalRequestsLastStatusesDto> GetRequestsLastStatus(
        GetApprovalRequestsLastStatusVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس موجودی زیرنوع تسهیلاتی در بسته اعتباری گروهی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IsExistFacilitySubTypeCodeInGroupCreditPackageDto> IsExistFacilitySubTypeCodeInGroupCreditPackage(
        IsExistFacilitySubTypeCodeInGroupCreditPackageVm vm,
        CancellationToken cancellationToken = default);
}