using DPK.Mosavabat.Models.DataModels.LoanBills;
using System.Collections.Generic;

namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class GetDailyPaidContractsTransactionsAmountsDm(
    List<GetLoanBillResultDm> loanBills)
{
    public List<GetLoanBillResultDm> LoanBills { get; } = loanBills;
}