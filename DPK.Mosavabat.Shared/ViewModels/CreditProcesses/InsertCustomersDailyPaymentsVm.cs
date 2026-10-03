namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses;

public class InsertCustomersDailyPaymentsVm
{
    public int BranchCode { get; set; }
    public string? RegionCode { get; set; }
    public string? CustomerNo { get; set; }
    public int CustomerType { get; set; }
    public string? Client { get; set; }
    public int ContractType { get; set; }
    public int SubContractType { get; set; }
    public string? C13 { get; set; }
    public string? C16 { get; set; }
}