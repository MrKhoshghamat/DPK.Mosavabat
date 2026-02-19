using DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

namespace DPK.Mosavabat.EntityFrameworkCore.Base;

public static class SeedHelper
{
    public static void SeedHostDb(IIocResolver iocResolver)
    {
        WithDbContext<MosavabatDbContext>(iocResolver, SeedHostDb);
    }

    public static void SeedHostDb(MosavabatDbContext context)
    {
        context.SuppressAutoSetTenantId = true;

        //Host seed
        new InitialHostDbBuilder(context).Create().GetAwaiter();
    }

    private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
        where TDbContext : DbContext
    {
        using var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>();
        using var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress);
        var context = uowManager.Object.Current
            .GetDbContext<TDbContext>(MultiTenancySides.Host);

        contextAction(context);

        uow.Complete();
    }
}