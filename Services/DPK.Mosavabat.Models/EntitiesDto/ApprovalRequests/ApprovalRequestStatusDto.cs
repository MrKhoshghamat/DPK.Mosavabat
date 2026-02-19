using System;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;

public record ApprovalRequestStatusDto(
    int IdStatusLog,
    DateTime EntryDate,
    DateTime ExitDate,
    [CanBeNull] string CurrentEmployee,
    [CanBeNull] string Activity,
    [CanBeNull] string Location,
    [CanBeNull] string StatusDescription);
