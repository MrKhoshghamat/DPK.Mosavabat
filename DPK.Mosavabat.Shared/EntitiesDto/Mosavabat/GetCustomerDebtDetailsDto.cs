namespace DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

public class GetCustomerDebtDetailsDto
{
    public string CustomerNumber { get; set; }
    public string C13 { get; set; }
    public long TotalDebt { get; set; }
    public string AllocatedDate { get; set; }
    public string ContractType { get; set; }
    public int? DateUpdate { get; set; }
    public int? TimeUpdate { get; set; }
}
