using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_Guarantor")]
public class PayGuarantor
{
    [XmlElement(ElementName = "GuarantorType")]
    public string GuarantorType { get; set; }

    [XmlElement(ElementName = "LastName")]
    public string LastName { get; set; }

    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "IssuancePeymentId")]
    public int IssuancePeymentId { get; set; }

    [XmlElement(ElementName = "Country")]
    public string Country { get; set; }

    [XmlElement(ElementName = "NationalCode")]
    public string NationalCode { get; set; }

    [XmlElement(ElementName = "IsDeleted")]
    public bool IsDeleted { get; set; }

    [XmlElement(ElementName = "Nationality")]
    public string Nationality { get; set; }

    [XmlElement(ElementName = "LcpId")]
    public string LcpId { get; set; }

    [XmlElement(ElementName = "FirstName")]
    public string FirstName { get; set; }

    [XmlElement(ElementName = "CreditRank")]
    public string? CreditRank { get; set; }

    [XmlElement(ElementName = "CreditScore")]
    public float? CreditScore { get; set; }
}