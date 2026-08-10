namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class CreatePaymentPermissionDataModel
{
    public int RequestFacilityCaseId { get; set; }
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
