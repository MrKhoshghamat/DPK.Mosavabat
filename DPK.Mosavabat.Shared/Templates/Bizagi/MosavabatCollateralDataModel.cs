using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "LCP_Collaterals")]
public class MosavabatCollateralDataModel
{
    [XmlElement(ElementName = "Amount")] 
    public string Amount { get; set; }

    [XmlElement(ElementName = "CollateralType")]
    public string? CollateralType { get; set; }

    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }
}