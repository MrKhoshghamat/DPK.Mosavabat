namespace DPK.Mosavabat.Api.IdentityServer;

/// <summary>
/// IdentityService Registration
/// </summary>
public static class IdentityServerRegistration
{
    //public static void Register(IServiceCollection services, IConfigurationRoot configuration, Action<IdentityServerOptions> setupOptions)
    //{

    //    services.AddIdentityServer(setupOptions)
    //        .AddDeveloperSigningCredential()
    //        .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    //        .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
    //        .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
    //        .AddInMemoryClients(IdentityServerConfig.GetClients(configuration));

    //}

    /// <summary>
    /// Main method
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void Register(IServiceCollection services, IConfigurationRoot configuration)
    {

        //Register(services, configuration, options => { });
    }
}