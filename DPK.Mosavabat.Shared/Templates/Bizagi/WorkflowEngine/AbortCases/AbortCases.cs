using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

public class AbortCases
{
    [XmlElement("Case")]
    public List<AbortCase> Case { get; set; }
}

