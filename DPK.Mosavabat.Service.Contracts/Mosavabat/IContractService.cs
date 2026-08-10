using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface IContractService
{
    Task<GetContractFileLinkDto?> GetContractFileLink(GetContractFileLinkVm viewModel, CancellationToken cancellationToken = default);

    Task<GetApprovalContractsDto> GetApprovalContracts(GetApprovalContractsDm dataModel, CancellationToken cancellationToken = default);

    Task<UpdateIsDeletedCuContractDto> UpdateIsDeletedCuContract(UpdateIsDeletedCuContractDm dataModel, CancellationToken cancellationToken = default);

    Task<GetContractTypeTitleByContractTypeIdDto> GetContractTypeTitleByContractTypeId(GetContractTypeTitleByContractTypeIdDm dataModel, CancellationToken cancellationToken = default);

    Task<GetSubContractTypeTitleBySubContractTypeIdDto> GetSubContractTypeTitleBySubContractTypeId(GetSubContractTypeTitleBySubContractTypeIdDm dataModel, CancellationToken cancellationToken = default);
}