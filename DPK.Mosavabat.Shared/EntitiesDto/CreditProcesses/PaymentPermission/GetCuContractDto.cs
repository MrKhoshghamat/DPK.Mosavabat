namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class GetCuContractDto
{
    public string C13 { get; set; }
    public long ContractAmount { get; set; }
    public long PrepaidAmount { get; set; }
    public string MosavabeNo { get; set; }
    public string? BranchCode { get; set; }
    public string? CustomerNo { get; set; }
    public string? ContractType { get; set; }
    public bool IsDeleted { get; set; }
}
