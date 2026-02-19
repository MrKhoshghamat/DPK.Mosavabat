namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class IsAllowedRecalculateAccountBalanceAverageDm(
    string mosavabeNo,
    long contractAmount)
{
    public string MosavabeNo { get; } = mosavabeNo;
    public long ContractAmount { get; } = contractAmount;
}