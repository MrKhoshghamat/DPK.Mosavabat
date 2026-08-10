using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "LCP_CollateralGroup")]
public class MosavabatCollateralGroupDataModel
{
    [XmlElement(ElementName = "Code")] public string Code { get; set; }

    [XmlElement(ElementName = "Priority")] public string Priority { get; set; }

    [XmlElement(ElementName = "Title")] public string Title { get; set; }
}