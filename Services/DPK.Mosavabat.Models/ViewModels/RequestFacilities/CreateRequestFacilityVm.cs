namespace DPK.Mosavabat.Models.ViewModels.RequestFacilities;

public class CreateRequestFacilityVm
{
    public string BranchCode { get; set; }
    public string CustomerNo { get; set; }
    public string AccountNo { get; set; }
    public string RequestDate { get; set; }
    public string RequestNo { get; set; }
    public string LegalName { get; set; }
    public long LcpId { get; set; }
    public bool IsDeleted { get; set; }
    public CreateCollateralsVm CreateCollateralVm { get; set; }
    public CreateGuarantorsVm CreateGuarantorsVm { get; set; }
    public CreateStatusVm CreateStatusVm { get; set; }
    public CreateSubjectFacilitiesVm CreateSubjectFacilitiesVm { get; set; }
    public CreateLcpFilesVm CreateLcpFilesVm { get; set; }
    public CreateRequestFacilityStatusesVm CreateRequestFacilityStatusesVm { get; set; }
}