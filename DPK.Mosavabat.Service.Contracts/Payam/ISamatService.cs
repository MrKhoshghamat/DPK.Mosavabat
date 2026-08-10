using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Payam;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Payam;

namespace DPK.Mosavabat.Services.Contracts.Payam;

public interface ISamatService
{
    Task<EstelamCheckDto> EstelamCheck(EstelamCheckVm viewModel, CancellationToken cancellationToken);
    Task<List<CbiTasDto>> GetCbiTasList(CbiTasVm? viewModel, CancellationToken cancellationToken = default);
    Task<List<BeneficiaryDto>> GetBeneficiaries(BeneficiaryVm viewModel, CancellationToken cancellationToken = default);
}