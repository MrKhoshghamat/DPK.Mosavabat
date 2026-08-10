using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

public class AbortCase
{
    [XmlElement("CaseId")]
    public int Id { get; set; }
}