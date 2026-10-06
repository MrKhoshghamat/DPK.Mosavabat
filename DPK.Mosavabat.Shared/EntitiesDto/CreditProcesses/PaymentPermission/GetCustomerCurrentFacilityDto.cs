namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class GetCustomerCurrentFacilityDto
{
    public long CustomerNo { get; set; }
    public long FacilityNo { get; set; }
    public long FacilityStatusCode { get; set; }
    public long CurrentInstallmentBalance { get; set; }
    public long NonCurrentInstallmentBalance { get; set; }
    public long TotalDebtBalance { get; set; }
    public int DateUpdate { get; set; }
    public int TimeUpdate { get; set; }
}