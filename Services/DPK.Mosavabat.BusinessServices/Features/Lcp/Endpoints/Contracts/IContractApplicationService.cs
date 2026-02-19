using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;

/// <summary>
/// سرویس های قرارداد
/// </summary>
public interface IContractApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس ساخت قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateLcpContractDto> CreateContract(
        CreateLcpContractVm vm,
        CancellationToken cancellationToken = default);
}