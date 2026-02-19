using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;

public record AccountBalanceAverageDto(
    [CanBeNull] string CustomerNo,
    [CanBeNull] string AccountNo,
    [CanBeNull] string PeriodTime,
    [CanBeNull] string Coefficient,
    [CanBeNull] string Amount,
    [CanBeNull] string MaxGrant,
    [CanBeNull] string AccountTitle,
    int Year,
    int Version);