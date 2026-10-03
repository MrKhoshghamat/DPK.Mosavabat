using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    [XmlRoot(ElementName = "LCP_BaratBeneficiary")]
    public class LCPBaratBeneficiaryDataModel
    {
        [XmlElement(ElementName = "Country")] public string Country { get; set; }
        [XmlElement(ElementName = "Description")] public string Description { get; set; }
        [XmlElement(ElementName = "FirstName")] public string FirstName { get; set; }
        [XmlElement(ElementName = "BeneficiaryType")] public string BeneficiaryType { get; set; }
        [XmlElement(ElementName = "LastName")] public string LastName { get; set; }
        [XmlElement(ElementName = "NationalCode")] public string NationalCode { get; set; }
        [XmlElement(ElementName = "Nationality")] public string Nationality { get; set; }
        [XmlElement(ElementName = "LcpId")] public string LcpId { get; set; }
        [XmlElement(ElementName = "IsDeleted")] public bool IsDeleted { get; set; }
        [XmlElement(ElementName = "LCP_BaratFactors")] public LcpBaratFactorsDataModel? Factors { get; set; } = default;
        
    }
}
