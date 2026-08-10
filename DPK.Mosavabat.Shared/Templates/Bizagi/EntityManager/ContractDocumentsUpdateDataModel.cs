using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_ContractDocument")]
public class ContractDocumentsUpdateDataModel
{
    [XmlElement(ElementName = "LCP_ContractDocument")] public List<ContractDocumentUpdateDataModel> RequestDocument { get; set; }
}