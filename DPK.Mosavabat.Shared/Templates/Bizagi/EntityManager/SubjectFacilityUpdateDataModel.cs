using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_SubjectFacility")]
public class SubjectFacilityUpdateDataModel
{
    //[XmlElement(ElementName = "LcpSubjectFacilitiesId")] public int Id { get; set; }
    [XmlElement(ElementName = "Amount")] public string Amount { get; set; }
    [XmlElement(ElementName = "ContractPeriod")] public string ContractPeriod { get; set; }
    [XmlElement(ElementName = "ContractPeriodType")] public string ContractPeriodType { get; set; }
    [XmlElement(ElementName = "Description")] public string Description { get; set; }
    [XmlElement(ElementName = "SubjectContract")] public string SubjectContract { get; set; }
    [XmlIgnore] public string ContractType { get; set; }
    [XmlElement(ElementName = "SubContractType")] public string? SubContractType { get; set; }
    //public MosavabatSubContractTypeDataModel SubContractType { get; set; }
    [XmlElement(ElementName = "FacilityGroup")] public string FacilityGroup { get; set; }
    [XmlElement(ElementName = "LCP_ContractDocument")] public ContractDocumentsUpdateDataModel RequestDocument { get; set; }
    [XmlElement(ElementName = "LcpId")] public long LcpId { get; set; }
    [XmlElement(ElementName = "IsDeleted")] public bool IsDeleted { get; set; }
}