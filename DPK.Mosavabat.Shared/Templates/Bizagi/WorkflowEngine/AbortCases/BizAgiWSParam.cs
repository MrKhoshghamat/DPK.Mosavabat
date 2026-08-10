using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizAgiWsParam
{
    [XmlElement(ElementName = "domain")] 
    public string Domain { get; set; }

    [XmlElement(ElementName = "userName")] 
    public string UserName { get; set; }

    public Cases Cases { get; set; }
}