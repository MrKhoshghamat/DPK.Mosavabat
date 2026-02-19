using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class LcpUpdateHistoryLogCommand(
    [CanBeNull] string title,
    [CanBeNull] string newValue,
    [CanBeNull] string oldValue,
    [CanBeNull] string approvalRequestId)
{
    [CanBeNull] public string Title { get; } = title;
    [CanBeNull] public string NewValue { get; } = newValue;
    [CanBeNull] public string OldValue { get; } = oldValue;
    [CanBeNull] public string ApprovalRequestId { get; } = approvalRequestId;
}