using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "DocumentType")]
public class MosavabatDocumentTypeDataModel
{
    [XmlElement(ElementName = "DocumentCode")]
    public string Code { get; set; }

    [XmlElement(ElementName = "DocumentType")]
    public string Title { get; set; }
}