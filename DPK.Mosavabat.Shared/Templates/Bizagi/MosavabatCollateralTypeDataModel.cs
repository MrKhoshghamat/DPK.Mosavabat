using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "CollateralType")]
public class MosavabatCollateralTypeDataModel
{
    [XmlElement(ElementName = "Code")] public string Code { get; set; }

    [XmlElement(ElementName = "Title")] public string Title { get; set; }

    [XmlElement(ElementName = "LCP_CollateralGroup")]
    public MosavabatCollateralGroupDataModel CollateralGroup { get; set; }
}