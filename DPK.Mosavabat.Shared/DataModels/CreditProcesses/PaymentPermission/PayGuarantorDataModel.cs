namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class PayGuarantorDataModel
{
    public string GuarantorType { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string Country { get; set; }
    public string NationalCode { get; set; }
    public string Nationality { get; set; }
    public string LcpId { get; set; }
    public string FirstName { get; set; }
    public string? CreditRank { get; set; }
    public float? CreditScore { get; set; }
}
