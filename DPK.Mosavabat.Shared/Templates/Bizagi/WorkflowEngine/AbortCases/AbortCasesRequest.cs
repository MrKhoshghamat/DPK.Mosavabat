using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

[XmlRoot("BizAgiWSParam")]
public class BizAgiAbortCasesRequest
{
    public string Domain { get; set; }
    public AbortCases Cases { get; set; }
}

