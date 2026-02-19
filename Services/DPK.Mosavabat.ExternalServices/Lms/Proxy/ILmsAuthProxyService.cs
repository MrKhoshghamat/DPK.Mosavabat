using DPK.Mosavabat.Common.Consts.Proxies.Lms;
using DPK.Mosavabat.Models.DataModels.Proxies.Shared;
using DPK.Mosavabat.Models.EntitiesDto.Lms;
using Refit;

namespace DPK.Mosavabat.ExternalServices.Lms.Proxy;

public interface ILmsAuthProxyService
{
    [Post(LmsProxyConsts.Urls.Login)]
    Task<ApiResponse<LmsTokenResponseDto>> LoginAsync(
        [Body] GetTokenDm dm,
        CancellationToken cancellationToken = default);
}