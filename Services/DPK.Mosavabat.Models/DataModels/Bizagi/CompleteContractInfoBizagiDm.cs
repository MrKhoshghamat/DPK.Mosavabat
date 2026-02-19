using System;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_Collaterals")]
public record CompleteContractInfoBizagiDm
{
    [XmlElement(ElementName = "Mab_Asl")]
    public decimal? MabAsl { get; init; }

    [XmlElement(ElementName = "CUApprovalSubject")]
    public int CuApprovalSubjectId { get; init; }

    [XmlElement(ElementName = "C10")]
    [CanBeNull] public string C10 { get; init; }

    [XmlElement(ElementName = "C13")]
    [CanBeNull] public string C13 { get; init; }

    [XmlElement(ElementName = "C16")]
    [CanBeNull] public string C16 { get; init; }

    [XmlElement(ElementName = "Date_g")]
    public DateTime? DateG { get; init; }

    [XmlElement(ElementName = "shob")]
    [CanBeNull] public string Shob { get; init; }
}