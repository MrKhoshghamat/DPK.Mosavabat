namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record CustomerPastDebtDto(
    string CustomerNo,
    long C13,
    long CurrentDebt,
    long NonCurrentDebt,
    string AllocatedDate,
    string Type);
