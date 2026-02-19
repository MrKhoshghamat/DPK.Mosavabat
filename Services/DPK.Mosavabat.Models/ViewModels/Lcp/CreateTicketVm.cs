namespace DPK.Mosavabat.Models.ViewModels.Lcp;

public class CreateTicketVm
{
    public bool HasSubjectFacilityId { get; set; }
    public int SubjectFacilityId { get; set; }
    public string CuApprovalSubjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RequestFacilityStatus { get; set; }
    public long Number { get; set; }
    public string TaskId { get; set; }
}