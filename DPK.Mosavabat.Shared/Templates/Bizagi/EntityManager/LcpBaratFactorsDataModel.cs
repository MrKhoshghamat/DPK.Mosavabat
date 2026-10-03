using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    [XmlRoot(ElementName = "LCP_BaratFactors")]
    public class LcpBaratFactorsDataModel
    {
        [XmlElement(ElementName = "LCP_BaratFactor")] public List<LcpBaratFactorDataModel> Factor { get; set; }
        public LcpBaratFactorsDataModel()
        {
            Factor = new List<LcpBaratFactorDataModel>();
        }
    }
}
