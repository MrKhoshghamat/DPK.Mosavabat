namespace DPK.Mosavabat.Entities.Models.LoanBill;

public class LoanBillData
{
    public int Id { get; set; }
    public string LoanId { get; set; }
    public string CustomerNo { get; set; }
    public string JsonData { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}