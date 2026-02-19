using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.WorkflowEngine.AbortCases;

public class Case
{
    [XmlElement(ElementName = "radNumber")]
    public string radNumber { get; set; }
    [XmlElement(ElementName = "abortReason")]
    public string abortReason { get; set; }
}