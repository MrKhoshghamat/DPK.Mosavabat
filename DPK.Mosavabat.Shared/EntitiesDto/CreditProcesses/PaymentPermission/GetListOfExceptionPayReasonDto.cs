namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class GetListOfExceptionPayReasonDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ExpireDate { get; set; }
}