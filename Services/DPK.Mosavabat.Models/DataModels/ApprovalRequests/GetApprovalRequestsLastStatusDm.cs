using System.Collections.Generic;

namespace DPK.Mosavabat.Models.DataModels.ApprovalRequests;

public class GetApprovalRequestsLastStatusDm(
    List<int> caseIds)
{
    public List<int> CaseIds { get; } = caseIds;
}