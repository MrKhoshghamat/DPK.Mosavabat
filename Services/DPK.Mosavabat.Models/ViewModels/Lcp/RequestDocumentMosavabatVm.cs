namespace DPK.Mosavabat.Models.ViewModels.Lcp;

public class RequestDocumentMosavabatVm
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public string DueDate { get; set; }
    public string DocumentNumber { get; set; }
    public string DocumentTypeCode { get; set; }
    public decimal PayAmount { get; set; }
    public string Owner { get; set; }
}