using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.ViewModels.AccountBalanceAverage;

public class GetRelatedAccountBalanceAverageVm
{
    [CanBeNull] public string CustomerNo { get; set; }
    [CanBeNull] public string AccountNo { get; set; }
    public int ApprovalRequestId { get; set; }
    public int CuMosavabeId { get; set; }
    public int TypeOfContractLength { get; set; }
    public int MaxContractLength { get; set; }
    [CanBeNull] public string Code { get; set; }
    [CanBeNull] public string CurrentMonth { get; set; }
    public int FacilityGroup { get; set; }
}