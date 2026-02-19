using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class RequestDocumentCommand(
    string amount,
    string dueDate,
    string documentNumber,
    [CanBeNull] string documentType,
    string payAmount,
    string owner,
    long lcpId,
    bool isDeleted)
{
    public string Amount { get; } = amount;
    public string DueDate { get; } = dueDate;
    public string DocumentNumber { get; } = documentNumber;
    [CanBeNull] public string DocumentType { get; } = documentType;
    public string PayAmount { get; } = payAmount;
    public string Owner { get; } = owner;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
}