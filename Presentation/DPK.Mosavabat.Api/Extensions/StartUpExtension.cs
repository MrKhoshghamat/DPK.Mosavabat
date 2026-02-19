namespace DPK.Mosavabat.Api.Extensions;

/// <summary>
/// Cors
/// </summary>
public static class StartUpExtension
{
    internal const string DefaultCorsPolicyName = "localhost";

    internal static void AddCorsPolicy(this IServiceCollection services, IConfigurationRoot appConfiguration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, builder =>
            {

                builder
                    .WithOrigins(
                         appConfiguration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

    }
}