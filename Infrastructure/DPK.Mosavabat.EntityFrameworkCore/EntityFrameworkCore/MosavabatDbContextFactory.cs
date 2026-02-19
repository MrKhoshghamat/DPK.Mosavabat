using DPK.Mosavabat.Common.Consts;
using DPK.Mosavabat.DomainEntities.Base;

namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

/* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
public class MosavabatDbContextFactory : IDesignTimeDbContextFactory<MosavabatDbContext>
{
    public MosavabatDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<MosavabatDbContext>();

        /*
         You can provide an environmentName parameter to the AppConfigurations.Get method.
         In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
         Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
         https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
         */
        var configuration = AppConfigurations.Get(
            WebContentDirectoryFinder.CalculateContentRootFolder(),
            addUserSecrets: true
        );

        MosavabatDbContextConfigurator.Configure(builder,
            configuration.GetConnectionString(MosavabatConsts.ConnectionStringName));

        return new MosavabatDbContext(builder.Options);
    }
}