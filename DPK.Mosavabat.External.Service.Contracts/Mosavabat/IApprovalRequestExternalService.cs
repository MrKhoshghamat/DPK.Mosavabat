using DPK.Mosavabat.Shared.EntitiesDto;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.External.Services.Contracts.Mosavabat;

public interface IApprovalRequestExternalService
{
    InitApprovalRequestModelDto? InitApprovalRequestModel(InitApprovalRequestModelVm viewModel);
    CreateApprovalsRequestDto? CreateApprovalsRequest(CreateApprovalsRequestVm viewModel);
    GetRequestFacilityCaseIdDto? GetRequestFacilityCaseId(GetRequestFacilityCaseIdVm viewModel);
}