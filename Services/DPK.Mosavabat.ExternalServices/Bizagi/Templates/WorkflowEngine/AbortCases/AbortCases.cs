using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.WorkflowEngine.AbortCases;

public class AbortCases
{
    [XmlElement("Case")]
    public List<AbortCase> Case { get; set; }
}

