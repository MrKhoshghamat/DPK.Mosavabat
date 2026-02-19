using DPK.Mosavabat.EntityFrameworkCore.Base.Seeder;
using DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

namespace DPK.Mosavabat.EntityFrameworkCore.Base;

public class InitialHostDbBuilder(MosavabatDbContext context)
{
    public async Task Create()
    {
        //new ProductManagementDataSeedContributor(_context).Create(); 
        new DefaultLanguagesCreator(context).Create();

        await context.SaveChangesAsync();
    }
}