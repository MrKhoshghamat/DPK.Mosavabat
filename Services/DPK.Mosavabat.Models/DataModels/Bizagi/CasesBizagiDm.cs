using System.Collections.Generic;
using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

public class CasesBizagiDm
{
    [XmlElement(ElementName = "cases")]
    public List<CaseBizagiDm> Entities { get; set; }
}