using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_ReqFacilityStatuses")]
public class RequestFacilityStatusesDataModel
{
    [XmlElement(ElementName = "LCP_ReqFacilityStatus")] public List<ReqFacilityStatusDataModel> ReqFacilityStatus { get; set; }
}