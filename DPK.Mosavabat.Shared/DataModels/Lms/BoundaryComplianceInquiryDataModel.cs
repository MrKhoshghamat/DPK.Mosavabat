namespace DPK.Mosavabat.Shared.DataModels.Lms;

public class BoundaryComplianceInquiryDataModel
{
    public int IssuancePaymentId { get; set; }
    public string NationalIdOrLegalId { get; set; }
    public int CustomerType { get; set; }
    public string BirthDateOrRegisterDate { get; set; }
    public long Amount { get; set; }
}
