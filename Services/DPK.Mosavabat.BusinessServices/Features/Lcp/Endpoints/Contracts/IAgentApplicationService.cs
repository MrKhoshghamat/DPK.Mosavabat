using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;

/// <summary>
/// سرویس های نماینده ثبت درخواست تسهیلات
/// </summary>
public interface IAgentApplicationService
    : IApplicationService
{
    /// <summary>
    /// تائید نماینده
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AcceptAgentDto> AcceptAgent(
        AcceptAgentVm vm,
        CancellationToken cancellationToken = default);
}