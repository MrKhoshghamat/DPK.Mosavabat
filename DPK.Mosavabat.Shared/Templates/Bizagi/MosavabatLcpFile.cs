using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "LCP_Files")]
public class LCPFile
{
    [XmlElement(ElementName = "FileLink")] public string FileLink { get; set; }

    [XmlElement(ElementName = "Name")] public string Name { get; set; }

    [XmlElement(ElementName = "Title")] public string Title { get; set; }
}