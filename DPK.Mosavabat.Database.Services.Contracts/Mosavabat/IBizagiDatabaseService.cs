using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface IBizagiDatabaseService : IMainDatabaseService
{
    // در این متد یک مدل پاس داده می شود که بر اساس آن AbortCase فراخوانی می شود
    Task<List<CaseForAbortCaseDto>?> ExtractAllCasesForAbortCase(CancellationToken cancellationToken);
    Task<IsWfUserDto> IsWfUser(IsWfUserVm vm, CancellationToken cancellationToken);
    Task<HasSupportRoleDto> HasSupportRole(HasSupportRoleVm vm, CancellationToken cancellationToken);
}