using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Endpoints.Contract;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.Models.ViewModels.RequestFacilities;
using Microsoft.AspNetCore.Mvc;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Endpoints.Service;

/// <summary>
/// سرویس های درخواست تسهیلات
/// </summary>
/// <param name="executionModule"></param>
public class RequestFacilityApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IRequestFacilityApplicationService
{
    /// <summary>
    /// سرویس ایجاد درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CreateRequestFacilityDto> Create(
        CreateRequestFacilityVm vm,
        CancellationToken cancellationToken)
    {
        return await executionModule.ExecuteCommandAsync(
                ObjectMapper.Map<CreateRequestFacilityCommand>(
                    vm),
                cancellationToken);
    }

    /// <summary>
    /// سرویس ویرایش درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<UpdateRequestFacilityDto> Update(
        UpdateRequestFacilityVm vm,
        CancellationToken cancellationToken)
    {
        return await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<UpdateRequestFacilityCommand>(
                vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس ویرایش شماره پیگیری درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<UpdateRequestFacilityRadNumberDto> UpdateLcpRequestRadNumber(
        UpdateRequestFacilityRadNumberVm vm,
        CancellationToken cancellationToken)
    {
        return await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<UpdateRequestFacilityRadNumberCommand>(
                vm),
            cancellationToken);
    }
}