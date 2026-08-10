using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface IBizagiService
{
    Task<List<CaseForAbortCaseDto>?> ExtractAllCasesForAbortCase(CancellationToken cancellationToken = default);
    Task<IsWfUserDto> IsWfUser(IsWfUserVm vm, CancellationToken cancellationToken = default);
    Task<HasSupportRoleDto> HasSupportRole(HasSupportRoleVm vm, CancellationToken cancellationToken = default);
}