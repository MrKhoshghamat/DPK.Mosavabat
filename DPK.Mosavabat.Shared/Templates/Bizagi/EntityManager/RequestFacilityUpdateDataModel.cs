using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_RequestFacility")]
public class RequestFacilityUpdateDataModel
{
    [XmlElement(ElementName = "GUID")] public string GUID { get; set; }
    [XmlElement(ElementName = "BranchCode")] public string BranchCode { get; set; }
    [XmlElement(ElementName = "CaseId")] public string CaseId { get; set; }
    [XmlElement(ElementName = "CustomerNo")] public string CustomerNo { get; set; }
    [XmlElement(ElementName = "AccountNo")] public string AccountNumber { get; set; }
    [XmlElement(ElementName = "LCP_Collaterals")] public CollateralsUpdateDataModel? Collaterals { get; set; }
    [XmlElement(ElementName = "LCP_Guarantors")] public GuarantorsUpdateDataModel? Guarantors { get; set; }
    [XmlElement(ElementName = "LCP_BaratBeneficiaries")] public LCPBaratBeneficiariesDataModel? BillOfExchanges { get; set; }
    [XmlElement(ElementName = "LCP_RFStatus")] public StatusUpdateDataModel RequestFacilityStatus { get; set; }
    [XmlElement(ElementName = "LCP_SubjectFacilitys")] public SubjectFacilitysUpdateDataModel? SubjectFacilities { get; set; }
    [XmlElement(ElementName = "RequestDate")] public string RequestDate { get; set; }
    [XmlElement(ElementName = "RequestNo")] public string RequestNo { get; set; }
    [XmlElement(ElementName = "LCP_Files")] public LcpFilesUpdateDataModel? LcpFiles { get; set; }
    [XmlElement(ElementName = "LegalName")] public string LegalName { get; set; }
    [XmlElement(ElementName = "LCP_ApprovelStatuse")] public string LcpApprovelStatuse { get; set; }
    [XmlElement(ElementName = "LcpId")] public long LcpId { get; set; }
    [XmlElement(ElementName = "IsDeleted")] public bool IsDeleted { get; set; }
    [XmlElement(ElementName = "LCP_ReqFacilityStatuses")] public RequestFacilityStatusesDataModel RequestFacilityStatuses { get; set; }
    [XmlElement(ElementName = "SummarizeDifference")] public string? SummarizeDifference { get; set; }
    [XmlElement(ElementName = "LCP_EditHistoryLogs")] public LcpHistoryLogsDataModel? LcpHistoryLogs { get; set; }
    //[XmlIgnore] public MosavabatLegalRankingDataModel? MosavabatLegalRanking { get; set; }
    //[XmlElement(ElementName = "LCP_CustomerRanks")] public LcpCustomerRanksDataModel? LcpCustomerRanks  { get; set; } = default;
}