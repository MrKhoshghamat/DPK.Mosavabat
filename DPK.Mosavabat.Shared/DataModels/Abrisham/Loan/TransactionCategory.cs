namespace DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;

public class TransactionCategory
{
    public HashSet<string> Codes { get; set; } = new();
    public HashSet<string> Descriptions { get; set; } = new();
}