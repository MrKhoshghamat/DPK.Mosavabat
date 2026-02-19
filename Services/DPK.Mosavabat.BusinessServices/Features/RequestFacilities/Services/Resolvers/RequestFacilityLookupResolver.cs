using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Services.Abstractions;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Services.Resolvers;

internal sealed class RequestFacilityLookupResolver(
    IRequestFacilitySqlService requestFacilitySqlService)
    : IRequestFacilityLookupResolver
{
    public Task<Result<GetUsernameByBranchCodeDto>> GetUsername(
        string branchCode,
        CancellationToken cancellationToken)
        => requestFacilitySqlService.GetUsernameByBranchCode(
            new GetUsernameByBranchCodeDm(
                int.Parse(branchCode),
                MosavabatClassName.LcpRequestFacility),
            cancellationToken);

    public Task<Result<GetCollateralTypeIdDto>> GetCollateralTypeId(
        string code,
        CancellationToken cancellationToken)
        => requestFacilitySqlService.GetCollateralTypeId(
            new GetCollateralTypeIdDm(int.Parse(code)),
            cancellationToken);

    public Task<Result<ExtractExchangeCommercialDocIdDto>> GetDocumentTypeId(
        string code,
        CancellationToken cancellationToken)
        => requestFacilitySqlService.ExtractExchangeCommercialDocId(
            new ExtractExchangeCommercialDocIdDm(code),
            cancellationToken);

    public Task<Result<ExtractSubContractTypeIdDto>> GetSubContractTypeId(
        string code,
        CancellationToken cancellationToken)
        => requestFacilitySqlService.ExtractSubContractTypeId(
            new ExtractSubContractTypeIdDm(int.Parse(code)),
            cancellationToken);

    public Task<Result<ExtractFacilityGroupIdDto>> GetFacilityGroupId(
        string code,
        CancellationToken cancellationToken)
        => requestFacilitySqlService.ExtractFacilityGroupId(
            new ExtractFacilityGroupIdDm(code),
            cancellationToken);
}
