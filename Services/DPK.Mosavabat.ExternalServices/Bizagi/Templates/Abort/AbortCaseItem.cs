using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Abort;

public class AbortCaseItem
{
    [XmlElement("radNumber")]
    public string RadNumber { get; set; }

    [XmlElement("abortReason")]
    public string AbortReason { get; set; }
}