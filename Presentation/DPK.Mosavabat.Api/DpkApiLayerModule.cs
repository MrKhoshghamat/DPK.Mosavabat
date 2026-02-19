using DPK.Mosavabat.Api.Extensions;
using DPK.Mosavabat.Api.NavigationBar;
using DPK.Mosavabat.Common.Consts;
using DPK.Mosavabat.DomainEntities.Base;
using DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

namespace DPK.Mosavabat.Api;

/// <summary>
/// Api layer module
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="env"></param>
[DependsOn(

    typeof(DpkAspNetCoreDataModule),
    typeof(DpkPrimitiveCoreModule),
    typeof(DpkDomainLayerModule),
    typeof(DpkEntityFrameworkLayerModule),
    typeof(BusinessServiceLayerModule)
)]
public class DpkApiLayerModule(IWebHostEnvironment env) : DpkModule

{
    private readonly IConfigurationRoot _appConfiguration = env.GetAppConfiguration();

    /// <summary>
    /// Pre initialize
    /// </summary>
    public override void PreInitialize()
    {
        Configuration.MultiTenancy.IsEnabled = false;


        IocManager.Register<IDpkSession, DpkMosavabatSessionBase>();

        //  Configuration.IocManager.Register<NullUnitOfWork>();
        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.DefaultNameOrConnectionString =
            _appConfiguration.GetConnectionString(MosavabatConsts.ConnectionStringName);


        Configuration.Modules.Primitive().LanguageManagement.EnableDbLocalization();


        Configuration.Navigation.Providers.Add<MosavabatNavigationProvider>();

        //عنوان ماژولی را که می خواهید به جای کنترلر استفاده شود در این قسمت وارد می نمایید
        //عنوان ماژول به صورت پیش فرض app  می باشد
        //در غیر این صورت نام آن را وارد نمایید مانند Business

        Configuration.Modules.DpkspNetCore()
            .CreateControllersForAppServices(
                typeof(BusinessServiceLayerModule).GetAssembly(),
                MosavabatConsts.ControllerModuleName
            );

        if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null &&
            bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"] ?? string.Empty))
        {
            ConfigureTokenAuth();
        }


        Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

        if (_appConfiguration["App:SendAllExceptionsToClients"] != null &&
            bool.Parse(_appConfiguration["App:SendAllExceptionsToClients"] ?? string.Empty))
        {
            Configuration.Modules.DpkWebCommon().SendAllExceptionsToClients = true;
        }

    }

    private void ConfigureTokenAuth()
    {
        IocManager.Register<TokenAuthConfiguration>();
        var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

        tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"] ?? string.Empty)
        );

        tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
        tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
        tokenAuthConfig.SigningCredentials =
            new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
        tokenAuthConfig.AccessTokenExpiration = MosavabatConsts.AccessTokenExpiration;
        tokenAuthConfig.RefreshTokenExpiration = MosavabatConsts.RefreshTokenExpiration;
    }
    /// <summary>
    /// Initialize
    /// </summary>
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(DpkApiLayerModule).GetAssembly());
    }
    /// <summary>
    /// Post initialize
    /// </summary>
    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(DpkApiLayerModule).Assembly);

        using var scope = IocManager.CreateScope();
        if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
        {
        }
    }

}