using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

public class Cases
{
    [XmlElement(ElementName = "cases")]
    public List<Case> Enities { get; set; }
}