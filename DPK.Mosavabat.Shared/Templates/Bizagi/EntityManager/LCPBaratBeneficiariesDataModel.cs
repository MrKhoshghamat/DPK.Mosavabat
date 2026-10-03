using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    [XmlRoot(ElementName = "LCP_BaratBeneficiaries")]
    public class LCPBaratBeneficiariesDataModel
    {
        [XmlElement(ElementName = "LCP_BaratBeneficiary")] public List<LCPBaratBeneficiaryDataModel> Beneficiary { get; set; }
    } 
}
