using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_ReqFacilityStatus")]
public class ReqFacilityStatusDataModel
{
    //[XmlElement(ElementName = "LCP_RequestFacility")] public string RequestFacility { get; set; }
    [XmlElement(ElementName = "LCP_RFStatus")] public string Status { get; set; }
}