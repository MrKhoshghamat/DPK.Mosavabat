using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface IAgentDatabaseService : IMainDatabaseService
{
    Task<UpdateCaseIdAndRadNumberInAgentRegistryDto> UpdateCaseIdAndRadNumberInAgentRegistry(UpdateCaseIdAndRadNumberInAgentRegistryDm dataModel, CancellationToken cancellationToken);

    Task<CheckAgentRegistryExistsDto> CheckAgentRegistryExists(CheckAgentRegistryExistsDm dataModel, CancellationToken cancellationToken);
    Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default);
    Task LcpAgentAbortCase(int caseId, CancellationToken cancellationToken);
}
