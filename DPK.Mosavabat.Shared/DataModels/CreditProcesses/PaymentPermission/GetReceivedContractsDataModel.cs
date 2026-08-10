using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class GetReceivedContractsDataModel
{
    public List<GetLoanBillDm> LoanBills { get; set; }
}