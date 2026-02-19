using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record GetApprovalRequestInfoDto(
    int ApprovalRequestId,
    int CaseId,
    [CanBeNull] string RadNumber);
