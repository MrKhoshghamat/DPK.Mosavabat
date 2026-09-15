using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat
{
    public interface IContractGroupDatabaseService : IMainDatabaseService
    {
        Task<ExtractContractGroupIdDto?> ExtractContractGroupId(ExtractContractGroupIdVm viewModel, CancellationToken cancellationToken = default);
    }
}
