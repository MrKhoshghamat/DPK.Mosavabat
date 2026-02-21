using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Endpoints.Contract;
using DPK.Mosavabat.Models.Commands.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.Queries.ApprovalRequests;
using DPK.Mosavabat.Models.ViewModels.ApprovalRequests;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Endpoints.Service;

/// <summary>
/// سرویس های درخواست صدور مصوبه
/// </summary>
/// <param name="executionModule"></param>
public class ApprovalRequestApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IApprovalRequestApplicationService
{
    /// <summary>
    /// سرویس های درخواست صدور مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateApprovalRequestDto> CreateApprovalRequest(
        CreateApprovalRequestVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<CreateApprovalRequestCommand>(vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس دریافت وضعیت های درخواست
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<ApprovalRequestStatusDto>> GetApprovalRequestStatuses(
        GetApprovalRequestStatusesVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetApprovalRequestStatusQuery>(vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس دریافت آخرین وضعیت درخواست ها
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetApprovalRequestsLastStatusesDto> GetRequestsLastStatus(
        GetApprovalRequestsLastStatusVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetApprovalRequestsLastStatusQuery>(vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس موجودی زیرنوع تسهیلاتی در بسته اعتباری گروهی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IsExistFacilitySubTypeCodeInGroupCreditPackageDto> IsExistFacilitySubTypeCodeInGroupCreditPackage(
        IsExistFacilitySubTypeCodeInGroupCreditPackageVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<IsExistFacilitySubTypeCodeInGroupCreditPackageQuery>(vm),
            cancellationToken);

        return executionResult.Value;
    }
}