namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;

public class PaymentPermissionExceptionReasonVM
{
    public string CustomerNumber { get; set; }
    public int BranchCode { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public int IssuanceLocation { get; set; }
    public int IssuanceExceptionId { get; set; }
    public DateTime LetterDate { get; set; }
    public string LetterNumber { get; set; }
    public int ExceptionCode { get; set; }
    public string RadNumber { get; set; }
    public bool IsDeleted { get; set; }
    public int Creator { get; set; }
    public string NationalCode { get; set; }
}
