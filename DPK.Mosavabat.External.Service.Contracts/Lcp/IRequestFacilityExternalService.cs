using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;

public interface IRequestFacilityExternalService
{
    Task<UpdateRequestFacilityDto?> AcceptRequestFacility(UpdateRequestFacilityVm viewModel, CancellationToken cancellationToken);
    Task<UpdateRequestFacilityRadNumberDto?> UpdateRadNumberRequestFacility(UpdateRequestFacilityRadNumberVm viewModel, CancellationToken cancellationToken);
}