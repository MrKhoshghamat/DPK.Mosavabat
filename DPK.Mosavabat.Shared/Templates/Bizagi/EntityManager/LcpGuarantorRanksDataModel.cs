using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    [XmlRoot(ElementName = "LCP_GuarantorRanks")]
    public class LcpGuarantorRanksDataModel
    {
        [XmlElement(ElementName = "LCP_GuarantorRank")] public List<LcpGuarantorRankDataModel> LcpGuarantorRank { get; set; }
        public LcpGuarantorRanksDataModel()
        {
            LcpGuarantorRank = new List<LcpGuarantorRankDataModel>();
        }
    }
}
