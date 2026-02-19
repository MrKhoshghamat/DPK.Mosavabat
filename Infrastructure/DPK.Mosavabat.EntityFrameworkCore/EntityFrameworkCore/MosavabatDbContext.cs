using DPK.Foundation.Identity.Models.Core;

namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

public class MosavabatDbContext(DbContextOptions<MosavabatDbContext> options)
    : DpkFullDbContext<Tenant, Role, User, MosavabatDbContext>(options)
{
    //در این بخش تمامی DbSet<Entity> های خود را وارد نمایید


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MosavabatDbContext).Assembly);

    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);

    }
}