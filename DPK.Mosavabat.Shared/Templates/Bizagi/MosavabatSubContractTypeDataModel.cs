using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "SubContractType")]
public class MosavabatSubContractTypeDataModel
{
    //[XmlElement(ElementName = "FacilityGroupCode")] 
    //public string FacilityGroupCode { get; set; }

    [XmlElement(ElementName = "ContractTemplateCode")]
    public string Code { get; set; }

    [XmlElement(ElementName = "ContractTemplateTitle")]
    public string Title { get; set; }
}