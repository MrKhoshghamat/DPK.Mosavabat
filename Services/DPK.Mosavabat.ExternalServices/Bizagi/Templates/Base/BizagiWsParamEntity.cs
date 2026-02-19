using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Base;

[XmlRoot("BizAgiWSParam")]
public class BizagiWsParamEntity<T> : BizagiWsParam
{
    public required Entities<T> Entities { get; set; }
}