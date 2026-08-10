using System.Xml;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.General.Supervisor;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General.Supervisor;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Mosavabat;

public class SupervisorService : ISupervisorService
{
    private readonly ISupervisorDatabaseService _supervisorDatabaseService;
    private readonly ILoggerManager _logger;

    public SupervisorService(
        ISupervisorDatabaseService supervisorDatabaseService,
        ILoggerManager logger)
    {
        _supervisorDatabaseService = supervisorDatabaseService;
        _logger = logger;
    }

    public async Task<FindRegionAndBrancheBySupervisorResponseModel?> FindRegionAndBrancheBySupervisor(FindRegionAndBrancheBySupervisorVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var response = await _supervisorDatabaseService.FindRegionAndBrancheBySupervisor(viewModel, cancellationToken);
        return response;
    }


}