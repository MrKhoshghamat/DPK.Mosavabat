namespace DPK.Mosavabat.Entities.Models.LoanBill;

public class LoanBillRequest
{
    public int Id { get; set; }
    public string CustomerNo { get; set; }
    public string LoanId { get; set; }
    public int DateUpdate { get; set; }
    public int TimeUpdate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsProcessing { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? NextRetryAt { get; set; }
}