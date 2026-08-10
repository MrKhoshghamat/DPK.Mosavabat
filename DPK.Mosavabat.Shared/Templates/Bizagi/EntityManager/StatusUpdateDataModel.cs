using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_RFStatus")]
public class StatusUpdateDataModel
{
    [XmlElement(ElementName = "Code")] public string Code { get; set; }

    [XmlElement(ElementName = "Title")] public string Title { get; set; }
}