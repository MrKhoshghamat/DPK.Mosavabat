using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using Microsoft.Extensions.Configuration;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class CollateralGroupDatabaseService : MainDatabaseService, ICollateralGroupDatabaseService
{
    public CollateralGroupDatabaseService(IConfiguration configuration) : base(configuration)
    {
    }
}