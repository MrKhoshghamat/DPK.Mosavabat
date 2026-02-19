using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

namespace DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;

public interface IApprovalRequestSqlService
    : ITransientDependency
{
    Task<Result<GetBranchCodeByLcpRequestFacilityIdDto>> GetBranchCodeByLcpRequestFacilityId(
        GetBranchCodeByLcpRequestFacilityIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetUsernameByBranchCodeDto>> GetUsernameByBranchCode(
        GetUsernameByBranchCodeDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<List<ApprovalRequestsLastStatusDto>>> GetRequestsLastStatuses(
        GetApprovalRequestsLastStatusDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>> IsExistFacilitySubTypeCodeInGroupCreditPackage(
        IsExistFacilitySubTypeCodeInGroupCreditPackageDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<List<ApprovalRequestStatusDto>>> GetApprovalRequestStatuses(
        GetApprovalRequestStatusesDm dm,
        CancellationToken cancellationToken = default);
}