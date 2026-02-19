using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;

/// <summary>
/// سرویس های درخواست تسهیلات
/// </summary>
public interface IRequestFacilityApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس تائید درخواست تسهیلات حقوقی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateLcpRequestFacilityStatusDto> AcceptRequestFacility(
        UpdateRequestFacilityStatusVm vm,
        CancellationToken cancellationToken);

    /// <summary>
    /// سرویس بروزرسانی شماره پیگیری درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateLcpRequestFacilityRadNumberDto> UpdateRequestFacilityRadNumber(
        UpdateLcpRequestFacilityRadNumberVm vm,
        CancellationToken cancellationToken);
}