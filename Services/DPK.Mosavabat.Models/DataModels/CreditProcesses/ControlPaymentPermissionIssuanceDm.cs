namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class ControlPaymentPermissionIssuanceDm(
    string mosavabeNo,
    long contractAmount,
    long? customerSharePrice,
    int? subContractType,
    string customerNo)
{
    public string MosavabeNo { get; } = mosavabeNo;
    public long ContractAmount { get; } = contractAmount;
    public long? CustomerSharePrice { get; } = customerSharePrice;
    public int? SubContractType { get; } = subContractType;
    public string CustomerNo { get; } = customerNo;
}