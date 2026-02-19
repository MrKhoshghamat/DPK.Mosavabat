namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class GetCustomerCurrentFacilityDm(
    long customerNo,
    long facilityNo,
    long facilityStatusCode,
    long currentInstallmentBalance,
    long nonCurrentInstallmentBalance,
    long totalDebtBalance)
{
    public long CustomerNo { get; } = customerNo;
    public long FacilityNo { get; } = facilityNo;
    public long FacilityStatusCode { get; } = facilityStatusCode;
    public long CurrentInstallmentBalance { get; } = currentInstallmentBalance;
    public long NonCurrentInstallmentBalance { get; } = nonCurrentInstallmentBalance;
    public long TotalDebtBalance { get; } = totalDebtBalance;
}