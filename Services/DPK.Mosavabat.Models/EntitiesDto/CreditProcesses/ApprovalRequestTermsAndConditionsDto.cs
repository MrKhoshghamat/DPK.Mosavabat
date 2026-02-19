using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record ApprovalRequestTermsAndConditionsDto(
    int Id,
    [CanBeNull] string Condition);