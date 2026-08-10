using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface IAgentService
{
    Task<RegisterAgentDto?> RegisterAgentAsync(RegisterAgentVm viewModel, CancellationToken cancellationToken = default);
    Task<UpdateCaseIdAndRadNumberInAgentRegistryDto> UpdateCaseIdAndRadNumberInAgentRegistry(UpdateCaseIdAndRadNumberInAgentRegistryVm viewModel, CancellationToken cancellationToken);
    Task<CheckAgentRegistryExistsDto> CheckAgentRegistryExists(CheckAgentRegistryExistsDm dataModel, CancellationToken cancellationToken);
    Task<RemoveAgentFromCartableDto> RemoveAgentFromCartable(AbortCasesVm viewModel, CancellationToken cancellationToken = default);
    Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default);
    Task LcpAgentAbortCase(int caseId, CancellationToken cancellationToken);
}
