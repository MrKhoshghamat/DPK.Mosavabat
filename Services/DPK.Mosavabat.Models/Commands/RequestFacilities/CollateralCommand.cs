using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CollateralCommand(
    [CanBeNull] string amount,
    [CanBeNull] string collateralType,
    [CanBeNull] string description,
    long lcpId,
    bool isDeleted)
{
    [CanBeNull] public string Amount { get; } = amount;
    [CanBeNull] public string CollateralType { get; } = collateralType;
    [CanBeNull] public string Description { get; } = description;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
}