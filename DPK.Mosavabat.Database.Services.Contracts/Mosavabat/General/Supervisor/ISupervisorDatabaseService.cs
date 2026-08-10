using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General.Supervisor;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface ISupervisorDatabaseService : IMainDatabaseService
{
    Task<FindRegionAndBrancheBySupervisorResponseModel?> FindRegionAndBrancheBySupervisor(FindRegionAndBrancheBySupervisorVm viewModel, CancellationToken cancellationToken);
}