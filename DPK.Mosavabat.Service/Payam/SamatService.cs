using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Services.Contracts.Payam;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Payam;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Payam;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Payam;

public class SamatService : ISamatService
{
    private readonly ISamatExternalService _samatExternalService;
    private readonly ILoggerManager _logger;

    public SamatService(
        ISamatExternalService samatExternalService,
        ILoggerManager logger)
    {
        _samatExternalService = samatExternalService;
        _logger = logger;
    }

    public async Task<EstelamCheckDto> EstelamCheck(EstelamCheckVm viewModel, CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel.NationalId));
        return await _samatExternalService.EstelamCheck(viewModel, cancellationToken);
    }

    public async Task<List<BeneficiaryDto>> GetBeneficiaries(BeneficiaryVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _samatExternalService.GetBeneficiaries(viewModel, cancellationToken);
    }

    public async Task<List<CbiTasDto>> GetCbiTasList(CbiTasVm? viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel.NationalId));
        return await _samatExternalService.GetCbiTas(viewModel, cancellationToken);
    }
}