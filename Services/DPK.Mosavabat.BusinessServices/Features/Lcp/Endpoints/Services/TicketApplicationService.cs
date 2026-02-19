using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Services;

/// <summary>
/// سرویس های تیکت
/// </summary>
public class TicketApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        ITicketApplicationService
{
    /// <summary>
    /// سرویس ساخت تیکت
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateTicketDto> CreateTicket(
        CreateTicketVm vm,
        CancellationToken cancellationToken)
    {
        return await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<CreateTicketCommand>(vm),
            cancellationToken);
    }
}