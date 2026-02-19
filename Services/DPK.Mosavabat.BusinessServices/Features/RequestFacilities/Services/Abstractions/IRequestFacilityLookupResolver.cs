using DPK.Mosavabat.Models.Commands.RequestFacilities;
using System.Threading.Tasks;
using System.Threading;
using DPK.Mosavabat.Common.Primitives;
using System.Collections.Generic;
using DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Services.Models;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Services.Abstractions;

public interface IRequestFacilityLookupResolver
{
    Task<Result<GetUsernameByBranchCodeDto>> GetUsername(
        string branchCode,
        CancellationToken cancellationToken);

    Task<Result<GetCollateralTypeIdDto>> GetCollateralTypeId(
        string code,
        CancellationToken cancellationToken);

    Task<Result<ExtractExchangeCommercialDocIdDto>> GetDocumentTypeId(
        string code,
        CancellationToken cancellationToken);

    Task<Result<ExtractSubContractTypeIdDto>> GetSubContractTypeId(
        string code,
        CancellationToken cancellationToken);

    Task<Result<ExtractFacilityGroupIdDto>> GetFacilityGroupId(
        string code,
        CancellationToken cancellationToken);
}