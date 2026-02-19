using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.Models.ViewModels.RequestFacilities;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Endpoints.Contract;

/// <summary>
/// سرویس های درخواست تسهیلات
/// </summary>
public interface IRequestFacilityApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس ایجاد درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateRequestFacilityDto> Create(
        CreateRequestFacilityVm vm,
        CancellationToken cancellationToken);

    /// <summary>
    /// سرویس ویرایش درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateRequestFacilityDto> Update(
        UpdateRequestFacilityVm vm,
        CancellationToken cancellationToken);

    /// <summary>
    /// سرویس ویرایش شماره پیگیری درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateRequestFacilityRadNumberDto> UpdateLcpRequestRadNumber(
        UpdateRequestFacilityRadNumberVm vm,
        CancellationToken cancellationToken);
}