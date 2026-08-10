using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

public class Case
{
    [XmlElement(ElementName = "radNumber")]
    public string radNumber { get; set; }
    [XmlElement(ElementName = "abortReason")]
    public string abortReason { get; set; }
}