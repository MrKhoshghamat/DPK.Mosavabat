using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_Files")]
public class LcpFilesUpdateDataModel
{
    [XmlElement(ElementName = "LCP_Files")] public List<LcpFileUpdateDataModel> LcpFiles { get; set; }
}