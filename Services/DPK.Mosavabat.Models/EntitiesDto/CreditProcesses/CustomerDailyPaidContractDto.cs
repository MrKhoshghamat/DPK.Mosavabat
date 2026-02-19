namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record CustomerDailyPaidContractDto(
    long C13,
    long C16,
    long CustomerNo,
    long ContractAmount,
    string Date,
    string ShamsiDate);