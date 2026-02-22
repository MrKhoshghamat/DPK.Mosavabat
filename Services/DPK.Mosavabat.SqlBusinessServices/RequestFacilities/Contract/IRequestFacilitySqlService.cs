using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

namespace DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

public interface IRequestFacilitySqlService
    : ITransientDependency
{
    Task<Result<UpdateLcpRequestFacilityDto>> UpdateLcpRequestFacility(
        UpdateLcpRequestFacilityDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<MapLcpRequestFacilityToApprovalRequestDto>> MapLcpRequestFacilityToApprovalRequest(
        MapLcpRequestFacilityToApprovalRequestDm dm,
        CancellationToken cancellationToken);

    Task<Result<GetCollateralTypeIdDto>> GetCollateralTypeId(
        GetCollateralTypeIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<ExtractExchangeCommercialDocIdDto>> ExtractExchangeCommercialDocId(
        ExtractExchangeCommercialDocIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<ExtractSubContractTypeIdDto>> ExtractSubContractTypeId(
        ExtractSubContractTypeIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<ExtractFacilityGroupIdDto>> ExtractFacilityGroupId(
        ExtractFacilityGroupIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetApprovalRequestIdDto>> GetApprovalRequestId(
        GetApprovalRequestIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<UpdateRequestFacilityStatusDto>> UpdateRequestFacilityStatus(
        UpdateRequestFacilityStatusDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<UpdateRequestFacilityRadNumberDto>> UpdateRequestFacilityRadNumber(
        UpdateRequestFacilityRadNumberDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetUserNameByCaseIdDto>> GetUserNameByCaseId(
        GetUserNameByCaseIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetUserIdByUsernameDto>> GetUserIdByUsername(
        GetUserIdByUsernameDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetRadNumberByCaseIdDto>> GetRadNumberByCaseId(
        GetRadNumberByCaseIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<AbortRequestFacilityCaseDto>> AbortRequestFacilityCase(
        AbortRequestFacilityCaseDm dm,
        CancellationToken cancellationToken = default);
}