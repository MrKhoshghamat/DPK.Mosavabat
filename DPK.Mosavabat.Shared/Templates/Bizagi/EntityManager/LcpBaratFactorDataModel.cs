using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    [XmlRoot(ElementName = "LCP_BaratFactor")]
    public class LcpBaratFactorDataModel
    {
        [XmlElement(ElementName = "FactorId")] public string FactorNumber { get; set; }
        [XmlElement(ElementName = "LcpId")] public int LcpId { get; set; }
    }
}
