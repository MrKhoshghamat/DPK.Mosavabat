using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class RemoveSettleContractsDataModel
{
    public List<GetLoanBillDm> LoanBills { get; set; }
    public List<GetCustomerCurrentFacilityDto>? Contracts { get; set; }
}