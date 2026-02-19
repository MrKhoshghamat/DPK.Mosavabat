using DPK.Mosavabat.Models.DataModels.Abrisham;
using System.Collections.Generic;

namespace DPK.Mosavabat.Models.DataModels.LoanBills;

public class GetLoanBillResultDm(
    string programName,
    string loanId,
    string billType,
    string terminal,
    string totalPhase,
    string phase,
    ICollection<TransactionDm> transactions,
    AbrishamResultDm result,
    string cicsTransId,
    string seqNo,
    string dataType,
    string acknowledge,
    string msgnbr)
{
    public string ProgramName { get; } = programName;
    public string LoanId { get; } = loanId;
    public string BillType { get; } = billType;
    public string Terminal { get; } = terminal;
    public string TotalPhase { get; } = totalPhase;
    public string Phase { get; } = phase;
    public ICollection<TransactionDm> Transactions { get; } = transactions;
    public AbrishamResultDm Result { get; } = result;
    public string CicsTransId { get; } = cicsTransId;
    public string SeqNo { get; } = seqNo;
    public string DataType { get; } = dataType;
    public string Acknowledge { get; } = acknowledge;
    public string Msgnbr { get; } = msgnbr;
}