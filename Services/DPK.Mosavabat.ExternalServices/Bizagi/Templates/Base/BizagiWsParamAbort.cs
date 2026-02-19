using System.Xml.Serialization;
using DPK.Mosavabat.ExternalServices.Bizagi.Templates.Abort;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Base;

[XmlRoot("BizAgiWSParam")]
public class BizagiWsParamAbort
{
    [XmlElement("domain")]
    public string Domain { get; set; }

    [XmlElement("userName")]
    public string UserName { get; set; }

    [XmlElement("Cases")]
    public AbortCases Cases { get; set; }
}