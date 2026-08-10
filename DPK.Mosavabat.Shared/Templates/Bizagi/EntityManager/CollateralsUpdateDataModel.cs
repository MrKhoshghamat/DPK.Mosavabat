using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_Collaterals")]
public class CollateralsUpdateDataModel
{
    [XmlElement(ElementName = "LCP_Collateral")] public List<CollateralUpdateDataModel?>? Collateral { get; set; }
}