using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat
{
    public interface IUserDatabaseService : IMainDatabaseService
    {
        Task<GetFacilityManagerOrSupervisorDto> GetFacilityManagerOrSupervisor(GetFacilityManagerOrSupervisorVm getFacilityManagerOrSupervisorVm, CancellationToken cancellationToken);
        Task<GetUserNameDto> GetUserName(GetUserNameVm getUserNameVm, CancellationToken cancellationToken);
        Task<GetCustomerEkDto> GetCustomerEk(GetCustomerEkVm viewModel, CancellationToken cancellationToken);
    }
}
