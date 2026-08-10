using DPK.Mosavabat.Shared.Utilities;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

public class MosavabatResponseDataModel
{
    public MosavabatResponseDataModel(int caseId, int radNumber)
    {
        CaseId = caseId;
        RadNumber = radNumber;
    }

    public MosavabatResponseDataModel(int caseId)
    {
        CaseId = caseId;
    }

    public int CaseId { get; set; }
    public int RadNumber { get; set; }
}