using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_Collateral")]
public class CollateralUpdateDataModel
{
    [XmlElement(ElementName = "Amount")] public string? Amount { get; set; }
    [XmlElement(ElementName = "CollateralType")] public string? CollateralType { get; set; }
    [XmlElement(ElementName = "Description")] public string? Description { get; set; }
    [XmlElement(ElementName = "LcpId")] public long? LcpId { get; set; }
    [XmlElement(ElementName = "IsDeleted")] public bool? IsDeleted { get; set; }
}