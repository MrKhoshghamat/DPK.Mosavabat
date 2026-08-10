namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class CreateIssuancePaymentPermissionDataModel
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int MosavabeNo { get; set; }
    public string? Customer { get; set; }
    public int BranchId { get; set; }
    public long ContractAmount { get; set; }
    public long BankSharePrice { get; set; }
    public long CustomerCashPrice { get; set; }
    public long CustomerNonCashPrice { get; set; }
    public string? Description { get; set; }
    public long FeeAmount { get; set; }
    public DateTime BaratDueDate { get; set; }
    public PayGuarantorsDataModel Guarantors { get; set; }
    public PayCollateralsDataModel Collaterals { get; set; }
    public PayBaratFactorsDataModel Factors { get; set; }
    public PayBaratBeneficiariesDataModel Beneficiaries { get; set; }
    public string LcpId { get; set; }
    public string? CreditRank { get; set; }
    public float? CreditScore { get; set; }
}
