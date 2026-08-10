using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager.SaveEntity;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizAgiWsParam
{
    [XmlElement(ElementName = "EntityData")]
    public EntityData EntityData { get; set; }
}