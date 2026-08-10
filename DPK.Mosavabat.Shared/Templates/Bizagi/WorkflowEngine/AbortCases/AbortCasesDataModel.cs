using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

public class AbortCasesDataModel
{
    public string userName { get; set; }
    public string userId { get; set; }
    public Cases Cases { get; set; }
}