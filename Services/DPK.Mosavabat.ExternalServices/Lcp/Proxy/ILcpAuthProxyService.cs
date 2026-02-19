using DPK.Mosavabat.Common.Consts.Proxies.Lcp;
using DPK.Mosavabat.Models.DataModels.Proxies.Shared;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using Refit;

namespace DPK.Mosavabat.ExternalServices.Lcp.Proxy;

public interface ILcpAuthProxyService
{
    [Post(LcpProxyConsts.Urls.AccountService.Login)]
    Task<ApiResponse<LcpTokenResponseDto>> LoginAsync(
        [Body] GetTokenDm dm,
        CancellationToken cancellationToken = default);
}