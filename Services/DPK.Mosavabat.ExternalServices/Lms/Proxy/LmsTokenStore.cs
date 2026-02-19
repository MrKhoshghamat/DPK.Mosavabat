using DPK.Foundation.UI;
using DPK.Mosavabat.Models.DataModels.Proxies.Shared;
using DPK.Mosavabat.Models.Settings.Lms;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DPK.Mosavabat.ExternalServices.Lms.Proxy;

public class LmsTokenStore(
    ILmsAuthProxyService authProxy,
    IMemoryCache cache,
    IOptions<LmsServiceSettings> settings)
{
    private const string CacheKey = "Lms_Access_Token_Cache_Key";

    public async Task<string?> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(
                CacheKey,
                out string? cachedToken)
            && !string
                .IsNullOrWhiteSpace(cachedToken))
        {
            return cachedToken;
        }

        var loginModel = new GetTokenDm(
            settings.Value.Username,
            settings.Value.Password,
            settings.Value.ClientId);

        var response = await authProxy
            .LoginAsync(
                loginModel,
                cancellationToken);

        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            throw new UserFriendlyException($"خطا در احراز هویت سرویس LMS. وضعیت: {response.StatusCode}");
        }

        var tokenDto = response.Content;

        if (string.IsNullOrWhiteSpace(tokenDto.AccessToken))
        {
            throw new UserFriendlyException("سرویس LMS پاسخ داد اما توکن دریافت نشد.");
        }

        var cacheDuration = tokenDto.ExpireInSeconds > 60
            ? TimeSpan.FromSeconds(tokenDto.ExpireInSeconds - 60)
            : TimeSpan.FromSeconds(tokenDto.ExpireInSeconds);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(cacheDuration)
            .SetPriority(CacheItemPriority.High);

        cache.Set(CacheKey, tokenDto.AccessToken, cacheOptions);

        return tokenDto.AccessToken;
    }
}