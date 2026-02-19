using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.RequestFacilities;

public class UpdateRequestFacilityVm
{
    public string BranchCode { get; set; }
    public string CustomerNo { get; set; }
    public string AccountNo { get; set; }
    public string RequestDate { get; set; }
    public string RequestNo { get; set; }
    public string LegalName { get; set; }
    public long LcpId { get; set; }
    public bool IsDeleted { get; set; }
    [CanBeNull] public string SummarizeDifference { get; set; }
    public UpdateCollateralsVm Collaterals { get; set; }
    public UpdateGuarantorsVm Guarantors { get; set; }
    public UpdateStatusVm Status { get; set; }
    public UpdateSubjectFacilitiesVm SubjectFacilities { get; set; }
    public UpdateLcpFilesVm LcpFiles { get; set; }
    public UpdateRequestFacilityStatusesVm RequestFacilityStatuses { get; set; }
}