using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Agents;
using DPK.Mosavabat.Models.ViewModels.Agents;

namespace DPK.Mosavabat.BusinessServices.Features.Agents.Endpoints.Contract;

/// <summary>
/// سرویس های نماینده
/// </summary>
public interface IAgentApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس ایجاد نماینده
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateAgentDto> Create(
        CreateAgentVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس حذف نماینده از کارتابل
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RemoveAgentFromCartableDto> RemoveAgentFromCartable(
        RemoveAgentFromCartableVm vm,
        CancellationToken cancellationToken = default);
}