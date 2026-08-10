namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class GetApprovalRequestInfoDto
{
    public int ApprovalRequestId { get; set; }
    public int CaseId { get; set; }
    public string? RadNumber { get; set; }
}
