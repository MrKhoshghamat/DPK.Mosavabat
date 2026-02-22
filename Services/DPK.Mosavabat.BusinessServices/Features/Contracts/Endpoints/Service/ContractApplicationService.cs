using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Contracts.Endpoints.Contract;
using DPK.Mosavabat.Models.Commands.Contracts;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;
using DPK.Mosavabat.Models.Queries.Contracts;
using DPK.Mosavabat.Models.ViewModels.Contracts;

namespace DPK.Mosavabat.BusinessServices.Features.Contracts.Endpoints.Service;

/// <summary>
/// سرویس های قرارداد
/// </summary>
/// <param name="executionModule"></param>
public class ContractApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IContractApplicationService
{
    /// <summary>
    /// سرویس بروز رسانی لینک فایل های قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UpdateContractFileLinkDto> UpdateContractFileLink(
        UpdateContractFileLinkVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<UpdateContractFileLinkCommand>(
                vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس دریافت قراردادهای مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<ContractDto>> GetApprovalContracts(
        GetApprovalContractsVm vm,
        CancellationToken cancellationToken)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetApprovalContractsQuery>(
                vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس دریافت عنوان نوع قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetContractTypeTitleByContractTypeIdDto> GetContractTypeTitleByContractTypeId(
        GetContractTypeTitleByContractTypeIdVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetContractTypeTitleByContractTypeIdQuery>(
                vm),
            cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس دریافت عنوان زیر نوع قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetSubContractTypeTitleBySubContractTypeIdDto> GetSubContractTypeTitleBySubContractTypeId(
        GetSubContractTypeTitleBySubContractTypeIdVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetSubContractTypeTitleBySubContractTypeIdQuery>(
                vm),
            cancellationToken);

        return executionResult.Value;
    }
}