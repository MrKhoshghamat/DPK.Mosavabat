using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager.SaveEntity;

[XmlRoot(ElementName = "EntityData")]
public class EntityData
{
    [XmlElement(ElementName = "EntityName")]
    public string EntityName { get; set; }
    [XmlElement(ElementName = "Filters")]
    public string Filters { get; set; }
}