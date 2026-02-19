using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.RequestFacilities;

public class RequestDocumentVm
{
    public string Amount { get; set; }
    public string DueDate { get; set; }
    public string DocumentNumber { get; set; }
    [CanBeNull] public string DocumentType { get; set; }
    public string PayAmount { get; set; }
    public string Owner { get; set; }
    public long LcpId { get; set; }
    public bool IsDeleted { get; set; }
}