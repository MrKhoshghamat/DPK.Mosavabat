using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Base;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizagiWsParamUpdate<T> : BizagiWsParam
{
    [XmlElement(ElementName = "ActivityData")]
    public required MosavabatActivityDataDataModel ActivityData { get; set; }

    [XmlElement(ElementName = "Entities")] public required Entities<T> Entities { get; set; }
}

[XmlRoot(ElementName = "ActivityData")]
public class MosavabatActivityDataDataModel
{
    [XmlElement(ElementName = "idCase")] public required string IdCase { get; set; }

    [XmlElement(ElementName = "taskId")] public required string TaskId { get; set; }
}