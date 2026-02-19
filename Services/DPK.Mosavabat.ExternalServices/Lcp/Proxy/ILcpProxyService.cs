using DPK.Mosavabat.Common.Consts.Proxies.Lcp;
using DPK.Mosavabat.Models.DataModels.Proxies.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using Refit;

namespace DPK.Mosavabat.ExternalServices.Lcp.Proxy;

public interface ILcpProxyService
{
    [Post(LcpProxyConsts.Urls.AgentService.AcceptAgent)]
    Task<ApiResponse<AcceptAgentDto>> AcceptAgentAsync(
        [Body] AcceptAgentProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LcpProxyConsts.Urls.ContractService.Create)]
    Task<ApiResponse<CreateLcpContractDto>> CreateContractAsync(
        [Body] CreateLcpContractProxyDm dm,
        CancellationToken cancellationToken = default);

    [Get(LcpProxyConsts.Urls.BoardMemberService.GetBoardMemberForMosavabat)]
    Task<ApiResponse<List<BoardMemberDto>>> GetBoardMembersAsync(
        [Query] GetBoardMembersProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LcpProxyConsts.Urls.TicketService.Create)]
    Task<ApiResponse<CreateTicketDto>> CreateTicketAsync(
        [Body] CreateTicketProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LcpProxyConsts.Urls.RequestFacilityService.UpdateStatus)]
    Task<ApiResponse<UpdateLcpRequestFacilityStatusDto>> UpdateLcpRequestFacilityStatusAsync(
        [Body] UpdateLcpRequestFacilityStatusProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LcpProxyConsts.Urls.RequestFacilityService.UpdateRadNumber)]
    Task<ApiResponse<UpdateLcpRequestFacilityRadNumberDto>> UpdateLcpRequestFacilityRadNumberAsync(
        [Body] UpdateLcpRequestFacilityRadNumberProxyDm dm,
        CancellationToken cancellationToken = default);
}