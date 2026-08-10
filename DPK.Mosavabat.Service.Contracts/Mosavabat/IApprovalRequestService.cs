using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface IApprovalRequestService
{
    Task<List<GetRequestStatusDto>?> GetRequestStatus(GetRequestStatusVm viewModel, CancellationToken cancellationToken = default);
    Task<GetRequestsLastStatusDto?> GetRequestsLastStatus(GetRequestsLastStatusVm viewModel, CancellationToken cancellationToken = default);
    Task<CreateApprovalRequestDto?> CreateApprovalRequest(CreateApprovalRequestVm viewModel, CancellationToken cancellationToken = default);
    Task<IsExistFacilitySubTypeCodeInGroupCreditPackageDto> IsExistFacilitySubTypeCodeInGroupCreditPackage(IsExistFacilitySubTypeCodeInGroupCreditPackageVm viewModel, CancellationToken cancellationToken = default);
}