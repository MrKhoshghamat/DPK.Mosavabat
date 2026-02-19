using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.AccountBalanceAverage;

public class GetRelatedAccountBalanceAverageDm(
    [CanBeNull] string customerNo,
    [CanBeNull] string accountNo,
    int approvalRequestId,
    int cuMosavabeId,
    int typeOfContractLength,
    int maxContractLength,
    [CanBeNull] string code,
    [CanBeNull] string currentMonth,
    int facilityGroup)
{
    [CanBeNull] public string CustomerNo { get; } = customerNo;
    [CanBeNull] public string AccountNo { get; } = accountNo;
    public int ApprovalRequestId { get; } = approvalRequestId;
    public int CuMosavabeId { get; } = cuMosavabeId;
    public int TypeOfContractLength { get; } = typeOfContractLength;
    public int MaxContractLength { get; } = maxContractLength;
    [CanBeNull] public string Code { get; } = code;
    [CanBeNull] public string CurrentMonth { get; } = currentMonth;
    public int FacilityGroup { get; } = facilityGroup;
}