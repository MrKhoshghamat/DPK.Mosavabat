using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.Base;

[XmlRoot(ElementName = "BizAgiWSParam")]
public class BizAgiWSParamCreate<T> : BizAgiWSParam
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