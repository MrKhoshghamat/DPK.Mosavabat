namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class GetListOfPaymentPermissionExceptionReasonDto
{
    public int Id { get; set; }
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
    public string NationalCode { get; set; }
    public  int Creator { get; set; }
}