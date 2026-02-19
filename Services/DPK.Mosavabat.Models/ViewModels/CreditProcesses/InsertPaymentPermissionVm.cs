namespace DPK.Mosavabat.Models.ViewModels.CreditProcesses;

public class InsertPaymentPermissionVm
{
    public string MosavabeNo { get; set; }
    public string BranchCode { get; set; }
    public string CustomerNo { get; set; }
    public string AccNo { get; set; }
    public int ContractType { get; set; }
    public int SubContractType { get; set; }
    public long ContractAmount { get; set; }
    public long PrepaidAmount { get; set; }
    public int? IssuancePaymentId { get; set; }
    public string? IssuancePaymentRadNumber { get; set; }
}