using System.Collections.Generic;
using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_RequestFacility")]
public class UpdateRequestFacilityBizagiDm
{
    [XmlElement(ElementName = "GUID")]
    public string Guid { get; set; }

    [XmlElement(ElementName = "BranchCode")]
    public string BranchCode { get; set; }

    [XmlElement(ElementName = "CaseId")]
    public string CaseId { get; set; }

    [XmlElement(ElementName = "CustomerNo")]
    public string CustomerNo { get; set; }

    [XmlElement(ElementName = "AccountNo")]
    public string AccountNumber { get; set; }

    [XmlElement(ElementName = "LCP_Collaterals")]
    public List<CollateralBizagiDm> LcpCollaterals { get; set; }

    [XmlElement(ElementName = "LCP_Guarantors")]
    public List<GuarantorBizagiDm> Guarantors { get; set; }

    [XmlElement(ElementName = "LCP_RFStatus")]
    public StatusBizagiDm RequestFacilityStatus { get; set; }

    [XmlElement(ElementName = "LCP_SubjectFacilitys")]
    public List<SubjectFacilityBizagiDm> SubjectFacilities { get; set; }

    [XmlElement(ElementName = "RequestDate")]
    public string RequestDate { get; set; }

    [XmlElement(ElementName = "RequestNo")]
    public string RequestNo { get; set; }

    [XmlElement(ElementName = "LCP_Files")]
    public List<LcpFileBizagiDm> LcpFiles { get; set; }

    [XmlElement(ElementName = "LcpId")]
    public long LcpId { get; set; }

    [XmlElement(ElementName = "LegalName")]
    public string LegalName { get; set; }
    [XmlElement(ElementName = "LCP_EditHistoryLogs")] public LcpHistoryLogsBizagiDm LcpHistoryLogs { get; set; }
}