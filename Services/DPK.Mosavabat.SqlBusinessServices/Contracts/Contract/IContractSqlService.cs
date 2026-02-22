using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.Contracts;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;

namespace DPK.Mosavabat.SqlBusinessServices.Contracts.Contract;

public interface IContractSqlService
    : ITransientDependency
{
    Task<Result<UpdateContractFileLinkDto>> UpdateContractFileLink(
        UpdateContractFileLinkDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<List<ContractDto>>> GetApprovalContracts(
        GetApprovalContractsDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetContractTypeTitleByContractTypeIdDto>> GetContractTypeTitleByContractTypeId(
        GetContractTypeTitleByContractTypeIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetSubContractTypeTitleBySubContractTypeIdDto>> GetSubContractTypeTitleBySubContractTypeId(
        GetSubContractTypeTitleBySubContractTypeIdDm dm,
        CancellationToken cancellationToken = default);
}