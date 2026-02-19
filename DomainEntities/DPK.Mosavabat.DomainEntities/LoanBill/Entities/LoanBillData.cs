using DPK.Foundation.Domain.Entities;

namespace DPK.Mosavabat.DomainEntities.LoanBill.Entities;

public class LoanBillData : Entity
{
    public string LoanId { get; set; }
    public string CustomerNo { get; set; }
    public string JsonData { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}