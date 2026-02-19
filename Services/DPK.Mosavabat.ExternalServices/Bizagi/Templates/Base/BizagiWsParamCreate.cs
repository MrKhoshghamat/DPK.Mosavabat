using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Base;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizagiWsParamCreate<T> : BizagiWsParam
{
    [XmlElement(ElementName = "Cases")] public Cases<T> Cases { get; set; }
}

[XmlRoot(ElementName = "Cases")]
public class Cases<T>
{
    [XmlElement(ElementName = "Case")] public Case<T> Case { get; set; }
}

[XmlRoot(ElementName = "Case")]
public class Case<T>
{
    [XmlElement(ElementName = "Process")] public string Process { get; set; }

    [XmlElement(ElementName = "Entities")] public Entities<T> Entities { get; set; }
}