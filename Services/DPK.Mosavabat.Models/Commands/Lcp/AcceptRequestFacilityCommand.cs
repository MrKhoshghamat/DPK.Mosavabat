using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.Lcp;

/// <summary>
/// ورودی هندلر سرویس تایید درخواست تسهیلات
/// </summary>
/// <param name="caseId"></param>
/// <param name="requestFacilityStatus"></param>
/// <param name="message"></param>
public class AcceptRequestFacilityCommand(
    int caseId,
    [CanBeNull] string requestFacilityStatus,
    [CanBeNull] string message)
    : CommandBase<UpdateLcpRequestFacilityStatusDto>
{
    public int CaseId { get; } = caseId;
    [CanBeNull] public string RequestFacilityStatus { get; } = requestFacilityStatus;
    [CanBeNull] public string Message { get; } = message;
}