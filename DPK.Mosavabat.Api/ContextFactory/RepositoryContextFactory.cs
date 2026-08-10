using DPK.Mosavabat.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DPK.Mosavabat.Api.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(
                configuration.GetConnectionString("sqlConnection"),
                b =>
                {
                    b.MigrationsAssembly("DPK.Mosavabat.Api");
                    b.CommandTimeout(90);
                }
            );

        return new RepositoryContext(builder.Options);
    }
}