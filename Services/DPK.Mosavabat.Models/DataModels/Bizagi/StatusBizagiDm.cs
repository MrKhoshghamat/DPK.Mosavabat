using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_RFStatus")]
public class StatusBizagiDm
{
    [XmlElement(ElementName = "Code")] public string Code { get; set; }

    [XmlElement(ElementName = "Title")] public string Title { get; set; }
}