using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.Queries.ApprovalRequests;
using DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.UseCases.Queries.IsExistFacilitySubTypeCodeInGroupCreditPackage;

/// <summary>
/// هندلر سرویس موجودی زیر نوع تسهیلات در بسته اعتباری گروهی
/// </summary>
internal class IsExistFacilitySubTypeCodeInGroupCreditPackageQueryHandler(
    IApprovalRequestSqlService approvalRequestSqlService,
    IMapper mapper)
    : IQueryHandler<
        IsExistFacilitySubTypeCodeInGroupCreditPackageQuery,
        Result<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>>
{
    public async Task<Result<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>> Handle(
        IsExistFacilitySubTypeCodeInGroupCreditPackageQuery query,
        CancellationToken cancellationToken)
    {
        return await approvalRequestSqlService
            .IsExistFacilitySubTypeCodeInGroupCreditPackage(
                mapper.Map<IsExistFacilitySubTypeCodeInGroupCreditPackageDm>(query),
                cancellationToken);
    }
}