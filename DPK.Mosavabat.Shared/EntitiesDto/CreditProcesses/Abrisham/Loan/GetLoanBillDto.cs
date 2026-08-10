using DPK.Mosavabat.Shared.EntitiesDto.Abrisham.Loan;

namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;

public class GetLoanBillDto
{
    public string ProgramName { get; set; }

    public string LoanId { get; set; }

    public string BillType { get; set; }

    public string Terminal { get; set; }

    public string TotalPhases { get; set; }

    public string Phase { get; set; }

    public ICollection<TransactionDto> Transactions { get; set; }

    public AbrishamResultDto Result { get; set; }

    public string CicsTransId { get; set; }

    public string SeqNo { get; set; }

    public string DataType { get; set; }

    public string Acknowledge { get; set; }

    public string Msgnbr { get; set; }
}