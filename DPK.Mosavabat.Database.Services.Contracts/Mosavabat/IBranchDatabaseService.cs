using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat
{
    public interface IBranchDatabaseService : IMainDatabaseService
    {
        Task<GetBranchCodeDto> GetBranchCode(GetBranchCodeVm viewModel, CancellationToken cancellationToken);

        Task<GetBranchCodeDto> GetBranchCode(GetBranchCodeFromPaymentPermissionByC13Vm viewModel,
            CancellationToken cancellationToken);
    }
}
