using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.WorkflowEngine.AbortCases;

public class AbortCase
{
    [XmlElement("CaseId")]
    public int Id { get; set; }
}