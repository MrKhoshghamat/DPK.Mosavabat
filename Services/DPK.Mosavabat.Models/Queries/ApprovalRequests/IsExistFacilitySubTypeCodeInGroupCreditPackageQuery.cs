using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Queries;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

namespace DPK.Mosavabat.Models.Queries.ApprovalRequests;

/// <summary>
/// ورودی هندلر سرویس بررسی موجودی زیرنوع تسهیلاتی در بسته اعتباری گروهی
/// </summary>
/// <param name="facilitySubTypeCode"></param>
public class IsExistFacilitySubTypeCodeInGroupCreditPackageQuery(
    int facilitySubTypeCode)
    : QueryBase<Result<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>>
{
    public int FacilitySubTypeCode { get; } = facilitySubTypeCode;
}