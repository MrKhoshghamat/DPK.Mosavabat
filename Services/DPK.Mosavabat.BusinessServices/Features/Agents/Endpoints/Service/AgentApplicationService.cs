using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Agents.Endpoints.Contract;
using DPK.Mosavabat.Models.Commands.Agents;
using DPK.Mosavabat.Models.EntitiesDto.Agents;
using DPK.Mosavabat.Models.ViewModels.Agents;

namespace DPK.Mosavabat.BusinessServices.Features.Agents.Endpoints.Service;

/// <summary>
/// سرویس های نماینده
/// </summary>
/// <param name="executionModule"></param>
public class AgentApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IAgentApplicationService
{
    /// <summary>
    /// سرویس ایجاد نماینده
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateAgentDto> Create(
        CreateAgentVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<CreateAgentCommand>(vm),
                cancellationToken);

        return executionResult.Value;
    }

    /// <summary>
    /// سرویس حذف نماینده از کارتابل
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<RemoveAgentFromCartableDto> RemoveAgentFromCartable(
        RemoveAgentFromCartableVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<RemoveAgentFromCartableCommand>(vm),
                cancellationToken);

        return executionResult.Value;
    }
}