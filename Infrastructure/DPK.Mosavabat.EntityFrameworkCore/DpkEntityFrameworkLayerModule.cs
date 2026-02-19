using DPK.Mosavabat.DomainEntities.Base;
using DPK.Mosavabat.EntityFrameworkCore.Base;
using DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

namespace DPK.Mosavabat.EntityFrameworkCore;

[DependsOn(
    typeof(DpkPrimitiveCoreEntityFrameworkCoreModule),
    typeof(DpkDomainLayerModule))]
public class DpkEntityFrameworkLayerModule : DpkModule
{
    /* Used it tests to skip DbContext registration, in order to use in-memory database of EF Core */
    public bool SkipDbContextRegistration { get; set; }

    public bool SkipDbSeed { get; set; }

    public override void PreInitialize()
    {

        //در صورتی که نیاز به استفاده چند کانتکس دیتا بیس باشد
        Configuration.ReplaceService<IConnectionStringResolver, MosavabatConnectionStringResolver>();

        //پیکربندی دیتابیس اول

        if (!SkipDbContextRegistration)
        {
            Configuration.Modules.DpkEfCore().AddDbContext<MosavabatDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    MosavabatDbContextConfigurator.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    MosavabatDbContextConfigurator.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }


        // Set this setting to true for enabling entity history.


        // Uncomment below line to write change logs for the entities below:
        // Configuration.EntityHistory.Selectors.Add("SfccEntities", EntityHistoryHelper.TrackedTypes);
        // Configuration.CustomConfigProviders.Add(new EntityHistoryConfigProvider(Configuration));
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(DpkEntityFrameworkLayerModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

        using var scope = IocManager.CreateScope();
        if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
        {
            SeedHelper.SeedHostDb(IocManager);
        }
    }
}