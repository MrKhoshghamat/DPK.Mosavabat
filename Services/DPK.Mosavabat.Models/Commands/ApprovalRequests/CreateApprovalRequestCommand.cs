using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.ApprovalRequests;

/// <summary>
/// ورودی هندلر سرویس ایجاد درخواست صدور مصوبه
/// </summary>
/// <param name="lcpRequestFacilityIdCase"></param>
/// <param name="lcpRequestFacilityRadNumber"></param>
/// <param name="lcpRequestFacilityId"></param>
/// <param name="guidLcpRequestFacility"></param>
public class CreateApprovalRequestCommand(
    int lcpRequestFacilityIdCase,
    int lcpRequestFacilityRadNumber,
    int lcpRequestFacilityId,
    [CanBeNull] string guidLcpRequestFacility)
    : CommandBase<Result<CreateApprovalRequestDto>>
{
    public int LcpRequestFacilityIdCase { get; } = lcpRequestFacilityIdCase;
    public int LcpRequestFacilityRadNumber { get; } = lcpRequestFacilityRadNumber;
    public int LcpRequestFacilityId { get; } = lcpRequestFacilityId;
    [CanBeNull] public string GuidLcpRequestFacility { get; } = guidLcpRequestFacility;
}