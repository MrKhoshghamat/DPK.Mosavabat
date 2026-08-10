using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_Guarantors")]
public class GuarantorsUpdateDataModel
{
    [XmlElement(ElementName = "LCP_Guarantor")] public List<GuarantorUpdateDataModel> Guarantor { get; set; }
}