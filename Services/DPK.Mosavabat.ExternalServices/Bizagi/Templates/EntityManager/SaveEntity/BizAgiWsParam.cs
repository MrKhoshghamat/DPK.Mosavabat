using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.EntityManager.SaveEntity;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizAgiWsParam
{
    [XmlElement(ElementName = "EntityData")]
    public EntityData EntityData { get; set; }
}