namespace DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

public record CalculateApprovalRequestUnusedBalanceDto(
    string Amount,
    long? RemainingContractsCount);
