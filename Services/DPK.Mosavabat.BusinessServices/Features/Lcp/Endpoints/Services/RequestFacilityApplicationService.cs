using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Services;

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
    /// سرویس تائید درخواست تسهیلات حقوقی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UpdateLcpRequestFacilityStatusDto> AcceptRequestFacility(
        UpdateRequestFacilityStatusVm vm,
        CancellationToken cancellationToken)
    {
        var executionResult = await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<AcceptRequestFacilityCommand>(vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس بروزرسانی شماره پیگیری درخواست تسهیلات
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UpdateLcpRequestFacilityRadNumberDto> UpdateRequestFacilityRadNumber(
        UpdateLcpRequestFacilityRadNumberVm vm,
        CancellationToken cancellationToken)
    {
        var executionResult = await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<UpdateRequestFacilityRadNumberCommand>(vm),
            cancellationToken);

        return executionResult.Value;
    }
}