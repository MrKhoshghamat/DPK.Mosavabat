using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_Files")]
public class LcpFileBizagiDm
{
    [XmlElement(ElementName = "LcpId")]
    public long LcpId { get; set; }

    [XmlElement(ElementName = "FileLink")] 
    public string FileLink { get; set; }

    [XmlElement(ElementName = "Name")] 
    public string Name { get; set; }

    [XmlElement(ElementName = "Title")] 
    public string Title { get; set; }
}