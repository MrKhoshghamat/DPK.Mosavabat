using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class GetPaidContractsDataModel
{
    public List<GetLoanBillDm> LoanBills { get; set; }
}