using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface ICollateralTypeDatabaseService : IMainDatabaseService
{
    Task<GetCollateralTypeIdDto?> GetCollateralTypeId(GetCollateralTypeIdVm viewModel, CancellationToken cancellationToken);
}