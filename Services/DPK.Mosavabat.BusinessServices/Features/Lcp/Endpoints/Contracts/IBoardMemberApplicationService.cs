using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.ViewModels.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.Endpoints.Contracts;

/// <summary>
/// سرویس های اعضای هیئت مدیره
/// </summary>
public interface IBoardMemberApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس دریافت اعضای هیئت مدیره
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BoardMemberDto>> GetBoardMembers(
        GetBoardMembersVm vm,
        CancellationToken cancellationToken = default);
}