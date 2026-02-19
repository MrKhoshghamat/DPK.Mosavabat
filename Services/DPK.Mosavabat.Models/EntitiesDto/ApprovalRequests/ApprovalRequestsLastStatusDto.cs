using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

public record ApprovalRequestsLastStatusDto(
    int CaseId,
    [CanBeNull] string StatusDescription);