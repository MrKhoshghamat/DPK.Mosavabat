using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.RequestFacilities;

public class CollateralVm
{
    [CanBeNull] public string Amount { get; set; }
    [CanBeNull] public string CollateralType { get; set; }
    [CanBeNull] public string Description { get; set; }
    public long LcpId { get; set; }
    public bool IsDeleted { get; set; }
}