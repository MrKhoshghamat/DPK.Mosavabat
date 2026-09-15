using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    public class LcpGuarantorRankDataModel
    {
        [XmlElement(ElementName = "ExternalRankAssignDate")] public DateTime? ExternalRankAssignDate { get; set; }
        [XmlElement(ElementName = "LCP_RequestFacility")] public int? LCP_Guarantor { get; set; }
        [XmlElement(ElementName = "CreditScore")] public decimal? CreditScore { get; set; }
        [XmlElement(ElementName = "ExternalScore")] public decimal? ExternalScore { get; set; }
        [XmlElement(ElementName = "ExternalRankFile")] public string? ExternalRankFile { get; set; }
        [XmlElement(ElementName = "CreditRank")] public string? CreditRank { get; set; }
        [XmlElement(ElementName = "ExternalShortTermRank")] public string? ExternalShortTermRank { get; set; }
        [XmlElement(ElementName = "IranianScore")] public decimal? IranianScore { get; set; }
        [XmlElement(ElementName = "ExternalLongTermRank")] public string? ExternalLongTermRank { get; set; }
        [XmlElement(ElementName = "ExternalRankExpireDate")] public DateTime? ExternalRankExpireDate { get; set; }
        [XmlElement(ElementName = "IranianRank")] public string? IranianRank { get; set; }
    }
}
