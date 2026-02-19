using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.WorkflowEngine.AbortCases;

public class Cases
{
    [XmlElement(ElementName = "cases")]
    public List<Case> Enities { get; set; }
}