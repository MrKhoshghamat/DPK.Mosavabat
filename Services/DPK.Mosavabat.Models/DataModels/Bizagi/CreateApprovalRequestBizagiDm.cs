using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "approvalsRequest")]
public class CreateApprovalRequestBizagiDm
{
    [XmlElement(ElementName = "RequestTypeDetail")]
    public string RequestTypeDetail { get; set; }

    [XmlElement(ElementName = "ApprovalRequestGroup")]
    public string ApprovalRequestGroup { get; set; }

    [XmlElement(ElementName = "LCP_RequestFacility")]
    public RequestFacilityBizagiDm RequestFacility { get; set; }
}