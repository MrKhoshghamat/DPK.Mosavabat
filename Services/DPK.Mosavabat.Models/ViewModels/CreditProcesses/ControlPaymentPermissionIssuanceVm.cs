namespace DPK.Mosavabat.Models.ViewModels.CreditProcesses;

public class ControlPaymentPermissionIssuanceVm
{
    public string MosavabeNo { get; set; }
    public long ContractAmount { get; set; }
    public long? CustomerSharePrice { get; set; }
    public int? SubContractType { get; set; }
    public string CustomerNo { get; set; }
}