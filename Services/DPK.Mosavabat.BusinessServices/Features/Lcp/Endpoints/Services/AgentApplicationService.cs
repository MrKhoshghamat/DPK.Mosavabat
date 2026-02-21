using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Services;

/// <summary>
/// سرویس های نماینده درخواست تسهیلات
/// </summary>
public class AgentApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IAgentApplicationService
{
    /// <summary>
    /// سرویس تائید نماینده
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<AcceptAgentDto> AcceptAgent(
        AcceptAgentVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<AcceptAgentCommand>(vm),
            cancellationToken);

        return executionResult.Value;
    }
}