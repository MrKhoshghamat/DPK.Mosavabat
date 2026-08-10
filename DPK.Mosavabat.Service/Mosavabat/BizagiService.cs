using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General;

namespace DPK.Mosavabat.Services.Mosavabat;

public class BizagiService : IBizagiService
{
    private readonly IBizagiDatabaseService _bizagiDatabaseService;
    private readonly ILoggerManager _logger;

    public BizagiService(IBizagiDatabaseService bizagiDatabaseService, ILoggerManager logger)
    {
        _bizagiDatabaseService = bizagiDatabaseService;
        _logger = logger;
    }

    public async Task<List<CaseForAbortCaseDto>?> ExtractAllCasesForAbortCase(CancellationToken cancellationToken = default)
    {
        return await _bizagiDatabaseService.ExtractAllCasesForAbortCase(cancellationToken);
    }

    public async Task<IsWfUserDto> IsWfUser(IsWfUserVm vm, CancellationToken cancellationToken = default)
    {
        return await _bizagiDatabaseService.IsWfUser(vm, cancellationToken);
    }

    public async Task<HasSupportRoleDto> HasSupportRole(HasSupportRoleVm vm, CancellationToken cancellationToken = default)
    {
        return await _bizagiDatabaseService.HasSupportRole(vm, cancellationToken);
    }
}