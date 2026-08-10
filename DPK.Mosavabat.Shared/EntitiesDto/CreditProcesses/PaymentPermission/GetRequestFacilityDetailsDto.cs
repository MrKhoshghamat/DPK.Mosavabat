namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

public class GetRequestFacilityDetailsDto
{
    public int BranchCode { get; set; }
    public int BranchId { get; set; }
    public string? Customer { get; set; }
    public int MosavabeNo { get; set; }
}
