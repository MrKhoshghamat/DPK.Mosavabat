using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.Base;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizAgiWSParamUpdate<T> : BizAgiWSParam
{
    [XmlElement(ElementName = "ActivityData")]
    public MosavabatActivityDataDataModel ActivityData { get; set; }

    [XmlElement(ElementName = "Entities")] public Entities<T> Entities { get; set; }
}

[XmlRoot(ElementName = "ActivityData")]
public class MosavabatActivityDataDataModel
{
    [XmlElement(ElementName = "idCase")] public string IdCase { get; set; }

    [XmlElement(ElementName = "taskId")] public string TaskId { get; set; }
}