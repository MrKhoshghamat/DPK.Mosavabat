using System.Xml.Serialization;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_Collaterals")]
public class CollateralBizagiDm
{
    [XmlElement(ElementName = "LcpId")]
    public long LcpId { get; set; }

    [XmlElement(ElementName = "Amount")]
    public string Amount { get; set; }

    [XmlElement(ElementName = "CollateralType")]
    [CanBeNull]
    public string CollateralType { get; set; }

    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }
}