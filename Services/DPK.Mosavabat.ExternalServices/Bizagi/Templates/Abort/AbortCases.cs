using System.Xml.Serialization;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Templates.Abort;

public class AbortCases
{
    [XmlElement("Case")]
    public List<AbortCaseItem> CaseList { get; set; } = [];
}