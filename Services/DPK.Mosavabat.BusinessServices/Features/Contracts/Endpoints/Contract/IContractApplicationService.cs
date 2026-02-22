using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;
using DPK.Mosavabat.Models.ViewModels.Contracts;

namespace DPK.Mosavabat.BusinessServices.Features.Contracts.Endpoints.Contract;

/// <summary>
/// سرویس های قرارداد
/// </summary>
public interface IContractApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس بروز رسانی لینک فایل های قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateContractFileLinkDto> UpdateContractFileLink(
        UpdateContractFileLinkVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت قراردادهای مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ContractDto>> GetApprovalContracts(
        GetApprovalContractsVm vm,
        CancellationToken cancellationToken);

    /// <summary>
    /// سرویس دریافت عنوان نوع قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetContractTypeTitleByContractTypeIdDto> GetContractTypeTitleByContractTypeId(
        GetContractTypeTitleByContractTypeIdVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت عنوان زیر نوع قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetSubContractTypeTitleBySubContractTypeIdDto> GetSubContractTypeTitleBySubContractTypeId(
        GetSubContractTypeTitleBySubContractTypeIdVm vm,
        CancellationToken cancellationToken = default);
}