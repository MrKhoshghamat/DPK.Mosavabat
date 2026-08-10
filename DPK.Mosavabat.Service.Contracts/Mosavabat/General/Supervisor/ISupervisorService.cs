using DPK.Mosavabat.Shared.DataModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General.Supervisor;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface ISupervisorService
{
    Task<FindRegionAndBrancheBySupervisorResponseModel?> FindRegionAndBrancheBySupervisor(FindRegionAndBrancheBySupervisorVm viewModel, CancellationToken cancellationToken = default);
}