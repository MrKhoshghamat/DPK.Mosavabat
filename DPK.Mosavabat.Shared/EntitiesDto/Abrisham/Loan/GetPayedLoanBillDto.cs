using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;

namespace DPK.Mosavabat.Shared.EntitiesDto.Abrisham.Loan;

public class GetPayedLoanBillDto
{
    public List<TransactionDto> Transactions { get; set; }
}