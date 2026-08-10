using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_SubjectFacilitys")]
public class SubjectFacilitysUpdateDataModel
{
    [XmlElement(ElementName = "LCP_SubjectFacility")] public List<SubjectFacilityUpdateDataModel> SubjectFacility { get; set; }
}