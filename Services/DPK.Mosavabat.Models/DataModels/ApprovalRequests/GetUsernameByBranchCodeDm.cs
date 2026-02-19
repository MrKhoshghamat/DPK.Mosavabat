using DPK.Mosavabat.Common.Consts.General;

namespace DPK.Mosavabat.Models.DataModels.ApprovalRequests;

public class GetUsernameByBranchCodeDm(
    int branchCode,
    MosavabatClassName mosavabatClassName)
{
    public int BranchCode { get; } = branchCode;
    public MosavabatClassName MosavabatClassName { get; } = mosavabatClassName;
}