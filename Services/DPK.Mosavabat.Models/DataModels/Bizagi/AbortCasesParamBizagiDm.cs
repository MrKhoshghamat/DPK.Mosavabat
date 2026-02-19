using System.Collections.Generic;
using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot("BizAgiWSParam")]
public class AbortCasesParamBizagiDm
{
    [XmlElement("domain")]
    public string Domain { get; set; } = "domain";

    [XmlElement("userName")]
    public string UserName { get; set; } = "admon";

    [XmlArray("Cases")]
    [XmlArrayItem("Case")]
    public List<AbortCaseBizagiDm> Cases { get; set; } = [];
}