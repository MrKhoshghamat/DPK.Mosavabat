namespace DPK.Mosavabat.Models.DataModels.Proxies.Lms;

public class InsertPaymentPermissionProxyDm(
    string mosavabeNo,
    string branchCode,
    string customerNo,
    int contractType,
    int? subContractType,
    long contractAmount,
    long prepaidAmount)
{
    public string MosavabeNo { get; } = mosavabeNo;
    public string BranchCode { get; } = branchCode;
    public string CustomerNo { get; } = customerNo;
    public int ContractType { get; } = contractType;
    public int? SubContractType { get; } = subContractType;
    public long ContractAmount { get; } = contractAmount;
    public long PrepaidAmount { get; } = prepaidAmount;
}