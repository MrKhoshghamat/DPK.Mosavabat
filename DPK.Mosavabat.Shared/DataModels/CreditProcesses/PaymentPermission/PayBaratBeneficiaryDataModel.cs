using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class PayBaratBeneficiaryDataModel
{
    public int ShahabId { get; set; }
    public string BeneficiaryType { get; set; }
    public string Name { get; set; }
    public string NationalCode { get; set; }
    public string LastName { get; set; }
    public string LcpId { get; set; }
    public string Iban { get; set; }
}
