using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;

/// <summary>
/// سرویس های تیکت
/// </summary>
public interface ITicketApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس ساخت تیکت
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateTicketDto> CreateTicket(
        CreateTicketVm vm,
        CancellationToken cancellationToken);
}