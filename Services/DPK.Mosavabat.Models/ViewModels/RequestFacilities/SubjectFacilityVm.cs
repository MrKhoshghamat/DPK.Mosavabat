namespace DPK.Mosavabat.Models.ViewModels.RequestFacilities;

public class SubjectFacilityVm
{
    public string Amount { get; set; }
    public string ContractPeriod { get; set; }
    public string ContractPeriodType { get; set; }
    public string Description { get; set; }
    public string SubjectContract { get; set; }
    public string? SubContractType { get; set; }
    public string FacilityGroup { get; set; }
    public RequestDocumentsVm RequestDocument { get; set; }
    public long LcpId { get; set; }
    public bool IsDeleted { get; set; }
}