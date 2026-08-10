using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.Base2;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizAgiWSParamUpdate2<T>
{
    [XmlElement(ElementName = "Entities")] public Entities2<T> Entities { get; set; }
}