using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Payam;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Payam;

namespace DPK.Mosavabat.External.Services.Contracts.Payam;

public interface ISamatExternalService
{
    Task<EstelamCheckDto> EstelamCheck(EstelamCheckVm viewModel, CancellationToken cancellationToken = default);
    Task<List<CbiTasDto>> GetCbiTas(CbiTasVm? viewModel, CancellationToken cancellationToken = default);
    Task<HasNonCurrentDebtDto> HasNonCurrentDebt(CbiTasVm? viewModel, CancellationToken cancellationToken = default);
    Task<List<BeneficiaryDto>> GetBeneficiaries(BeneficiaryVm viewModel, CancellationToken cancellationToken = default);
}