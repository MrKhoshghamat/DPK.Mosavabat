using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class RemoveSettleAllContractsDataModel
{

    public List<GetLoanBillDm> LoanBills { get; set; }
    public List<GetCustomerDebtDetailsDto> Contracts { get; set; }
}
