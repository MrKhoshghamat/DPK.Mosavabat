using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

public class CaseBizagiDm
{
    [XmlElement(ElementName = "radNumber")]
    public string RadNumber { get; set; }

    [XmlElement(ElementName = "abortReason")]
    public string AbortReason { get; set; }
}