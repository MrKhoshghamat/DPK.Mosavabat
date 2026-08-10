using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "LCP_RFStatus")]
public class MosavabatStatusDataModel
{
    [XmlElement(ElementName = "Code")] public string Code { get; set; }

    [XmlElement(ElementName = "Title")] public string Title { get; set; }
}