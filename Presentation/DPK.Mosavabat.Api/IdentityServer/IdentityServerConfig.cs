using Duende.IdentityServer.Models;

namespace DPK.Mosavabat.Api.IdentityServer;

/// <summary>
/// IdentityService config
/// </summary>
public static class IdentityServerConfig
{
    /// <summary>
    /// Get api resources
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<ApiResource> GetApiResources()
    {
        var list = new List<ApiResource>
        {
            new("default-api", "Default (all) API")
            {
                Description = "AllFunctionalityYouHaveInTheApplication",
                ApiSecrets = {new Secret("secret")},
                Scopes =
                [
                    "default-api"
                ]
            }
        };
        return list;
    }

    /// <summary>
    /// Get api scopes
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return
            [
                new ApiScope(name: "default-api", displayName: "Default (all) API")
            ];
    }

    /// <summary>
    /// Get identity resources
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return
            [
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            ];
    }

    /// <summary>
    /// Get clients
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IEnumerable<Client> GetClients(IConfigurationRoot configuration)
    {
        return configuration.GetSection("IdentityServer:Clients")
            .GetChildren()
            .Select(child => new Client
            {
                ClientId = child["ClientId"] ?? string.Empty,
                ClientName = child["ClientName"],
                AllowedGrantTypes = child.GetSection("AllowedGrantTypes")
                    .GetChildren()
                    .Select(c => c.Value)
                    .ToArray()!,
                RequireConsent = bool.Parse(child["RequireConsent"] ?? "false"),
                AllowOfflineAccess = bool.Parse(child["AllowOfflineAccess"] ?? "false"),
                ClientSecrets = child.GetSection("ClientSecrets")
                    .GetChildren()
                    .Select(secret => new Secret(secret["Value"].Sha256()))
                    .ToArray(),
                AllowedScopes = child.GetSection("AllowedScopes").GetChildren().Select(c => c.Value).ToArray()!,
                RedirectUris = child.GetSection("RedirectUris").GetChildren().Select(c => c.Value).ToArray()!,
                PostLogoutRedirectUris = child.GetSection("PostLogoutRedirectUris")
                    .GetChildren()
                    .Select(c => c.Value)
                    .ToArray()!
            })
            .ToList();
    }
}