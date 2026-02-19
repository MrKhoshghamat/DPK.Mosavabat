using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_Guarantors")]
public class GuarantorBizagiDm
{
    [XmlElement(ElementName = "LcpId")]
    public long LcpId { get; set; }

    [XmlElement(ElementName = "Country")] 
    public string Country { get; set; }

    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "FirstName")]
    public string FirstName { get; set; }

    [XmlElement(ElementName = "GuarantorType")]
    public string GuarantorType { get; set; }

    [XmlElement(ElementName = "LastName")] 
    public string LastName { get; set; }

    [XmlElement(ElementName = "LegalGuarantorType")]
    public string LegalGuarantorType { get; set; }

    [XmlElement(ElementName = "NationalCode")]
    public string NationalCode { get; set; }

    [XmlElement(ElementName = "Nationality")]
    public string Nationality { get; set; }
}