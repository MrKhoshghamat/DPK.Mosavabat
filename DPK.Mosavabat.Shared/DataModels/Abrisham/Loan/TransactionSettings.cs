namespace DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;

public class TransactionSettings
{
    public TransactionCategory Settled { get; set; } = new();
    public TransactionCategory Received { get; set; } = new();
    public TransactionCategory Paid { get; set; } = new();
}