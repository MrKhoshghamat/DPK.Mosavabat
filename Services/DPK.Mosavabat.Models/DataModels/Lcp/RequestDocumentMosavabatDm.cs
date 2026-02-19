namespace DPK.Mosavabat.Models.DataModels.Lcp;

public class RequestDocumentMosavabatDm(
    long id,
    decimal amount,
    string dueDate,
    string documentNumber,
    string documentTypeCode,
    decimal payAmount,
    string owner)
{
    public long Id { get; } = id;
    public decimal Amount { get; } = amount;
    public string DueDate { get; } = dueDate;
    public string DocumentNumber { get; } = documentNumber;
    public string DocumentTypeCode { get; } = documentTypeCode;
    public decimal PayAmount { get; } = payAmount;
    public string Owner { get; } = owner;
}