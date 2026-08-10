using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_Files")]
public class LcpFileUpdateDataModel
{
    [XmlElement(ElementName = "FileLink")] public string FileLink { get; set; }
    [XmlElement(ElementName = "FileDocumentLink")] public string FileDocumentLink { get; set; }
    [XmlElement(ElementName = "Name")] public string Name { get; set; }
    [XmlElement(ElementName = "Title")] public string Title { get; set; }
    [XmlElement(ElementName = "LcpId")] public long LcpId { get; set; }
    [XmlElement(ElementName = "IsDeleted")] public bool IsDeleted { get; set; }
}