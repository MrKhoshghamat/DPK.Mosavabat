using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.Queries.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Services;

/// <summary>
/// سرویس های اعضای هیئت مدیره
/// </summary>
public class BoardMemberApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        IBoardMemberApplicationService
{
    /// <summary>
    /// سرویس دریافت اعضای هیئت مدیره
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<BoardMemberDto>> GetBoardMembers(
        GetBoardMembersVm vm,
        CancellationToken cancellationToken = default)
    {
        var executionResult = await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetBoardMembersQuery>(vm),
            cancellationToken);

        return executionResult.Value;
    }
}