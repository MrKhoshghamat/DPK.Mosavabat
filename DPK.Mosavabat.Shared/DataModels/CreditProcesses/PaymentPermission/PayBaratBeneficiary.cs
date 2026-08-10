using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_BaratBeneficiary")]
public class PayBaratBeneficiary
{
    [XmlElement(ElementName = "ShahabId")]
    public int ShahabId { get; set; }

    [XmlElement(ElementName = "BeneficiaryType")]
    public string BeneficiaryType { get; set; }

    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "IssuancePeymentId")]
    public int IssuancePeymentId { get; set; }

    [XmlElement(ElementName = "NationalCode")]
    public string NationalCode { get; set; }

    [XmlElement(ElementName = "LastName")]
    public string LastName { get; set; }

    [XmlElement(ElementName = "LcpId")]
    public string LcpId { get; set; }

    [XmlElement(ElementName = "IBAN")]
    public string Iban { get; set; }
}