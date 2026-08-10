using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface IContractDatabaseService : IMainDatabaseService
{
    Task<UpdateIsDeletedCuContractDto> UpdateIsDeletedCuContract(UpdateIsDeletedCuContractDm dataModel, CancellationToken cancellationToken = default);
    Task UpdateCuCustomerInCuContract(UpdateCuCustomerInCuContractDm dataModel, CancellationToken cancellationToken= default);
    Task<GetContractTypeTitleByContractTypeIdDto> GetContractTypeTitleByContractTypeId(GetContractTypeTitleByContractTypeIdDm dataModel, CancellationToken cancellationToken = default);

    Task<GetSubContractTypeTitleBySubContractTypeIdDto> GetSubContractTypeTitleBySubContractTypeId(GetSubContractTypeTitleBySubContractTypeIdDm dataModel, CancellationToken cancellationToken = default);
}
