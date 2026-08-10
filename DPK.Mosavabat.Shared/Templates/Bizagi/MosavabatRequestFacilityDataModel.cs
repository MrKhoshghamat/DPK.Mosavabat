using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "LCP_RequestFacility")]
public class MosavabatRequestFacilityDataModel
{
    [XmlElement(ElementName = "GUID")]
    public string GUID { get; set; }
    [XmlElement(ElementName = "BranchCode")]
    public string BranchCode { get; set; }

    [XmlElement(ElementName = "CaseId")] 
    public string CaseId { get; set; }

    [XmlElement(ElementName = "CustomerNo")]
    public string CustomerNo { get; set; }

    [XmlElement(ElementName = "AccountNo")]
    public string AccountNumber { get; set; }

    [XmlElement(ElementName = "LCP_Collaterals")]
    public List<MosavabatCollateralDataModel> LCPCollaterals { get; set; }

    [XmlElement(ElementName = "LCP_Guarantors")]
    public List<MosavabatGuarantorDataModel> Guarantors { get; set; }

    [XmlElement(ElementName = "LCP_RFStatus")]
    public MosavabatStatusDataModel RequestFacilityStatus { get; set; }

    [XmlElement(ElementName = "LCP_SubjectFacilitys")]
    public List<MosavabatSubjectFacilityDataModel> SubjectFacilities { get; set; }

    [XmlElement(ElementName = "RequestDate")]
    public string RequestDate { get; set; }

    [XmlElement(ElementName = "RequestNo")]
    public string RequestNo { get; set; }

    [XmlElement(ElementName = "LCP_Files")]
    public List<LCPFile> LcpFiles { get; set; }

    [XmlElement(ElementName = "LegalName")]
    public string LegalName { get; set; }
    //[XmlElement(ElementName = "LCP_ApprovelStatuse")]
    //public string ApprovalStatus { get; set; }
}