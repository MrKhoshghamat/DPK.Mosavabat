namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

/// <summary>
/// در صورتی از این حالت استفاده می شود که بیش از یک دیتا کانتکس وجود داشته باشد
///  در صورت استفاده از یک کانتکس نیازی به پیاده‌سازی نمی باشد
/// </summary>
public class MosavabatConnectionStringResolver(
    IDpkStartupConfiguration configuration,
    IHostingEnvironment hostingEnvironment)
    : DefaultConnectionStringResolver(configuration)
{
    private readonly IConfigurationRoot _appConfiguration = AppConfigurations.Get(hostingEnvironment.ContentRootPath,
        hostingEnvironment.EnvironmentName);
}