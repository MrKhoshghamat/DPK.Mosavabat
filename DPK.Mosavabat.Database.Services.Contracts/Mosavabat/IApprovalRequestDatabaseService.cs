using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface IApprovalRequestDatabaseService : IMainDatabaseService
{
    Task<List<GetRequestStatusDto>?> GetRequestStatus(GetRequestStatusVm viewModel, CancellationToken cancellationToken);
    Task<List<GetRequestLastStatusDto>> GetRequestsLastStatusBulk(List<int> caseIds, CancellationToken cancellationToken = default);
    Task<IsExistFacilitySubTypeCodeInGroupCreditPackageDto> IsExistFacilitySubTypeCodeInGroupCreditPackage(IsExistFacilitySubTypeCodeInGroupCreditPackageVm viewModel, CancellationToken cancellationToken);
}