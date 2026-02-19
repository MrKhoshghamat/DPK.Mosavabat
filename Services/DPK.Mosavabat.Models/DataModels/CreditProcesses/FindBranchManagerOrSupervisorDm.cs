using DPK.Mosavabat.Common.Consts.General;

namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class FindBranchManagerOrSupervisorDm(
    int branchCode,
    MosavabatClassName mosavabatClassName)
{
    public int BranchCode { get; } = branchCode;
    public MosavabatClassName MosavabatClassName { get; } = mosavabatClassName;
}