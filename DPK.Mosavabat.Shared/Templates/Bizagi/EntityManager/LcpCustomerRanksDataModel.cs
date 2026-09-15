using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    [XmlRoot(ElementName = "LCP_CustomerRanks")]
    public class LcpCustomerRanksDataModel
    {
        [XmlElement(ElementName = "LCP_CustomerRank")] public List<LcpCustomerRankDataModel> LcpCustomerRank { get; set; }
        public LcpCustomerRanksDataModel()
        {
            LcpCustomerRank = new List<LcpCustomerRankDataModel>();
        }
    }
}
