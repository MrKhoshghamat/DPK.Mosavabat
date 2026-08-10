using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.Base2;

[XmlRoot(ElementName = "Entities")]
public class Entities2<T>
{
    public T Dynamic { get; set; }
}