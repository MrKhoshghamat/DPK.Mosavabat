using System;

namespace DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

public record CreateApprovalRequestDto(
    long CaseId,
    string RadNumber,
    DateTime ProcessDate,
    string Message);