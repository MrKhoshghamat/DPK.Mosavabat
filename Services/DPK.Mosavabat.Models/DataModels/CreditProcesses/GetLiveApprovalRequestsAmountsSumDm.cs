namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class GetLiveApprovalRequestsAmountsSumDm(
    string customerNo,
    int idApprovalRequest)
{
    public string CustomerNo { get; } = customerNo;
    public int IdApprovalRequest { get; } = idApprovalRequest;
}