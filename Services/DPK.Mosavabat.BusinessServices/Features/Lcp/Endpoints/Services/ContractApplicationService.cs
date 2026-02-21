using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Services;

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
    /// سرویس ساخت قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateLcpContractDto> CreateContract(
        CreateLcpContractVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<CreateContractCommand>(vm),
            cancellationToken);

        return executionResult.Value;
    }
}