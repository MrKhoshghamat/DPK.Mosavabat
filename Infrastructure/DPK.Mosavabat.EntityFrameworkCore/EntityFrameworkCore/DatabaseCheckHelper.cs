namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

public class DatabaseCheckHelper(
    IDbContextProvider<MosavabatDbContext> dbContextProvider,
    IUnitOfWorkManager unitOfWorkManager)
    : ITransientDependency
{
    public bool Exist(string? connectionString)
    {
        if (connectionString.IsNullOrEmpty())
        {
            //connectionString is null for unit tests
            return true;
        }

        try
        {
            using var uow = unitOfWorkManager.Begin();
            // Switching to host is necessary for single tenant mode.
            using (unitOfWorkManager.Current.SetTenantId(null))
            {
                dbContextProvider.GetDbContext().Database.OpenConnection();
                uow.Complete();
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
    public async Task<bool> ExistAsync(string connectionString)
    {
        if (connectionString.IsNullOrEmpty())
        {
            //connectionString is null for unit tests
            return true;
        }

        try
        {
            using var uow = unitOfWorkManager.Begin();
            // Switching to host is necessary for single tenant mode.
            using (unitOfWorkManager.Current.SetTenantId(null))
            {
                var context = await dbContextProvider.GetDbContextAsync();
                await context.Database.OpenConnectionAsync();
                await uow.CompleteAsync();
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}