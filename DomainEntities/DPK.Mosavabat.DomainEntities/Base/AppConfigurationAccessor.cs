namespace DPK.Mosavabat.DomainEntities.Base;

public class AppConfigurationAccessor(IWebHostEnvironment env)
    : IAppConfigurationAccessor, ISingletonDependency
{
    public IConfigurationRoot Configuration { get; } = env.GetAppConfiguration();
}