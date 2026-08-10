namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class CustomerContractPastDebtDto
{
    public string CustNo { get; set; }
    public long C13 { get; set; }
    public long CurrentDebt { get; set; }
    public long NonCurrentDebt { get; set; }
    public string AllocatedDate { get; set; }
    public string Type { get; set; }
}
