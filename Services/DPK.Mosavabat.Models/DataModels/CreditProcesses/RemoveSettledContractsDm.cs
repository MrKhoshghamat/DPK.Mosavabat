using DPK.Mosavabat.Models.DataModels.LoanBills;
using System.Collections.Generic;

namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class RemoveSettledContractsDm(
    List<GetLoanBillResultDm> loanBills,
    List<GetCustomerCurrentFacilityDm> contracts)
{
    public List<GetLoanBillResultDm> LoanBills { get; } = loanBills;
    public List<GetCustomerCurrentFacilityDm> Contracts { get; } = contracts;
}