namespace DPK.Mosavabat.Models.EntitiesDto.LoanBills;

public record TransactionDto(
    string PaymentDate,
    string PaymentTime,
    string Amount,
    string PaymentGateway,
    string BranchCode,
    string Description,
    string Indicator,
    string TransactionCode);
