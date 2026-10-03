using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;

[XmlRoot(ElementName = "LCP_Guarantor")]
public class GuarantorUpdateDataModel
{
    [XmlElement(ElementName = "Country")] public string Country { get; set; }
    [XmlElement(ElementName = "Description")] public string Description { get; set; }
    [XmlElement(ElementName = "FirstName")] public string FirstName { get; set; }
    [XmlElement(ElementName = "GuarantorType")] public string GuarantorType { get; set; }
    [XmlElement(ElementName = "LastName")] public string LastName { get; set; }
    [XmlElement(ElementName = "LegalGuarantorType")] public string LegalGuarantorType { get; set; }
    [XmlElement(ElementName = "NationalCode")] public string NationalCode { get; set; }
    [XmlElement(ElementName = "Nationality")] public string Nationality { get; set; }
    [XmlElement(ElementName = "LcpId")] public long LcpId { get; set; }
    [XmlElement(ElementName = "IsDeleted")] public bool IsDeleted { get; set; }
    //[XmlElement(ElementName = "LCP_GuarantorRanks")] public LcpGuarantorRanksDataModel? GuarantorRanks { get; set; } = default;
    ///// <summary>
    ///// اگر حقیقی باشد
    ///// </summary>
    //[XmlIgnore] public ModavabatRankingDataModel? IranianRanking { get; set; }
    ///// <summary>
    ///// اگر حقوقی باشد
    ///// </summary>
    //[XmlIgnore] public MosavabatLegalRankingDataModel? MosavabatLegalRanking { get; set; }

}