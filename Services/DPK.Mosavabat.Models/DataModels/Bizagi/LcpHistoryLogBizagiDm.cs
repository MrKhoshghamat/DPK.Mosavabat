using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_EditHistoryLog")]
public class LcpHistoryLogBizagiDm
{
    [XmlElement(ElementName = "Title")] public string? Title { get; set; }
    [XmlElement(ElementName = "NewValue")] public string? NewValue { get; set; }
    [XmlElement(ElementName = "OldValue")] public string? OldValue { get; set; }
    [XmlElement(ElementName = "approvalsRequest")] public string? ApprovalRequestId { get; set; }
}