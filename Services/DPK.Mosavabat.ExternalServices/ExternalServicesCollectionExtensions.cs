using DPK.Mosavabat.ExternalServices.Lcp.HttpHandler;
using DPK.Mosavabat.ExternalServices.Lcp.Proxy;
using DPK.Mosavabat.ExternalServices.Lms.HttpHandler;
using DPK.Mosavabat.ExternalServices.Lms.Proxy;
using DPK.Mosavabat.ExternalServices.Logging;
using DPK.Mosavabat.Models.Settings.Lcp;
using DPK.Mosavabat.Models.Settings.Lms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace DPK.Mosavabat.ExternalServices;

public static class ExternalServicesCollectionExtensions
{
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static IServiceCollection AddLmsProxyServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<LmsServiceSettings>(configuration.GetSection("LmsService"));

        var retryPolicy = GetRetryPolicy();

        services.AddMemoryCache();
        services.AddTransient<LmsAuthHeaderHandler>();
        services.AddTransient<LmsTokenStore>();

        var refitSettings = new RefitSettings
        {
            UrlParameterKeyFormatter = new CamelCaseUrlParameterKeyFormatter()
        };

        services.AddRefitClient<ILmsAuthProxyService>(refitSettings)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<LmsServiceSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<ExternalServiceLoggingHandler>()
            .AddPolicyHandler(retryPolicy);

        services.AddRefitClient<ILmsProxyService>(refitSettings)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<LmsServiceSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
            })
            .AddHttpMessageHandler<LmsAuthHeaderHandler>()
            .AddHttpMessageHandler<ExternalServiceLoggingHandler>()
            .AddPolicyHandler(retryPolicy);

        return services;
    }

    public static IServiceCollection AddLcpProxyServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<LcpServiceSettings>(configuration.GetSection("LcpService"));

        var retryPolicy = GetRetryPolicy();

        services.AddMemoryCache();
        services.AddTransient<LcpAuthHeaderHandler>();
        services.AddTransient<LcpTokenStore>();

        var refitSettings = new RefitSettings
        {
            UrlParameterKeyFormatter = new CamelCaseUrlParameterKeyFormatter()
        };

        services.AddRefitClient<ILcpAuthProxyService>(refitSettings)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<LcpServiceSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<ExternalServiceLoggingHandler>()
            .AddPolicyHandler(retryPolicy);

        services.AddRefitClient<ILcpProxyService>(refitSettings)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<LcpServiceSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
            })
            .AddHttpMessageHandler<ExternalServiceLoggingHandler>()
            .AddHttpMessageHandler<LcpAuthHeaderHandler>()
            .AddPolicyHandler(retryPolicy);

        return services;
    }
}
