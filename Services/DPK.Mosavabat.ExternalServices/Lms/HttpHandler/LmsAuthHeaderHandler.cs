using DPK.Mosavabat.ExternalServices.Lms.Proxy;
using System.Net.Http.Headers;

namespace DPK.Mosavabat.ExternalServices.Lms.HttpHandler;

public class LmsAuthHeaderHandler(
    LmsTokenStore tokenStore)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await tokenStore.GetAccessTokenAsync(
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(
                accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                accessToken);
        }

        var response = await base.SendAsync(
            request,
            cancellationToken);

        return response;
    }
}