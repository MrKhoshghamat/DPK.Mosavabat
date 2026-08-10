using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

public class MosavabatApprovalsRequestUpdateDataModel
{
    public string TaskId { get; set; }
    public string IdCase { get; set; }
    public MosavabatApprovalsRequest Entity { get; set; }
}