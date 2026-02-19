namespace DPK.Mosavabat.Models.DataModels.LoanBills;

public class TransactionDm(
    string paymentDate,
    string paymentTime,
    string amount,
    string paymentGateway,
    string branchCode,
    string description,
    string indicator,
    string transactionCode)
{
    public string PaymentDate { get; } = paymentDate;
    public string PaymentTime { get; } = paymentTime;
    public string Amount { get; } = amount;
    public string PaymentGateway { get; } = paymentGateway;
    public string BranchCode { get; } = branchCode;
    public string Description { get; } = description;
    public string Indicator { get; } = indicator;
    public string TransactionCode { get; } = transactionCode;
}