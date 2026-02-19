using DPK.Foundation.Domain.Entities;

namespace DPK.Mosavabat.DomainEntities.LoanBill.Entities;

public class LoanBillRequest : Entity
{
    public string CustomerNo { get; set; }
    public string LoanId { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsProcessing { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RetryCount { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime? NextRetryAt { get; set; }
}