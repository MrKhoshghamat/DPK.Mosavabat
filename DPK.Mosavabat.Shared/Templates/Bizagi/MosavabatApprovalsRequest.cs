using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "approvalsRequest")]
public class MosavabatApprovalsRequest
{
    [XmlElement(ElementName = "RequestTypeDetail")]
    public string RequestTypeDetail { get; set; }
    [XmlElement(ElementName = "ApprovalRequestGroup")]
    public string ApprovalRequestGroup { get; set; }
    [XmlElement(ElementName = "LCP_RequestFacility")]
    public MosavabatRequestFacilityDataModel RequestFacility { get; set; }
    [XmlElement(ElementName = "CreditScore")] 
    public float? CreditScore { get; set; }
    [XmlElement(ElementName = "CreditRank")] 
    public string? CreditRank { get; set; }
}
