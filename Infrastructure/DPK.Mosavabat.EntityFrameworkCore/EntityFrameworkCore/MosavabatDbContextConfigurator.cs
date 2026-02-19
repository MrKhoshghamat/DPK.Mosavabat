using DPK.Mosavabat.EntityFrameworkCore.Logging;

namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

public static class MosavabatDbContextConfigurator
{
    public static void Configure(DbContextOptionsBuilder<MosavabatDbContext> builder, string? connectionString)
    {
        builder.UseSqlServer(connectionString, sqlBuilder =>
            {
                // sqlBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            })
            .AddInterceptors(new SqlServiceLoggingInterceptor());
    }

    public static void Configure(DbContextOptionsBuilder<MosavabatDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection, sqlBuilder =>
            {
                // sqlBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            })
            .AddInterceptors(new SqlServiceLoggingInterceptor());
    }
}