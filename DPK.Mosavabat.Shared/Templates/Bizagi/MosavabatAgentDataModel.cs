using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "AgentRegistry")]
public class MosavabatAgentDataModel
{
    [XmlElement(ElementName = "BranchCode")]
    public string BranchCode { get; set; }

    [XmlElement(ElementName = "AgentNationalCode")]
    public string NationalCode { get; set; }

    [XmlElement(ElementName = "CreationDate")]
    public string CreationDate { get; set; }

    [XmlElement(ElementName = "AgentLastName")]
    public string LastName { get; set; }

    [XmlElement(ElementName = "AgentMobile")]
    public string Mobile { get; set; }

    [XmlElement(ElementName = "AgentName")]
    public string Name { get; set; }

    [XmlElement(ElementName = "LegalEmail")]
    public string LegalEmail { get; set; }

    [XmlElement(ElementName = "LegalNationalId")]
    public string LegalNationalId { get; set; }

    [XmlElement(ElementName = "LegalName")]
    public string LegalName { get; set; }
}