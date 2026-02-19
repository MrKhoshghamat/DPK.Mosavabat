using System.Collections.Generic;
using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_EditHistoryLogs")]
public class LcpHistoryLogsBizagiDm
{
    [XmlElement(ElementName = "LCP_EditHistoryLog")]
    public List<LcpHistoryLogBizagiDm> LcpHistoryLogs { get; set; }
}