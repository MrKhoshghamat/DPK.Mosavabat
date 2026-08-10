using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.Lcp;

[XmlRoot(ElementName = "LCP_EditHistoryLogs")]
public class LcpHistoryLogsDataModel
{
    [XmlElement(ElementName = "LCP_EditHistoryLog")] public List<LcpHistoryLogDataModel>? LcpHistoryLogs { get; set; }
}
