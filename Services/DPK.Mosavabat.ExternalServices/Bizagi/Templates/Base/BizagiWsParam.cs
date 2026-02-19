using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Base;

public class BizagiWsParam
{
    [XmlElement(ElementName = "domain")] public string Domain { get; set; }

    [XmlElement(ElementName = "userName")] public string UserName { get; set; }
}

[XmlRoot(ElementName = "Entities")]
public class Entities<T>
{
    public T Dynamic { get; set; }
}