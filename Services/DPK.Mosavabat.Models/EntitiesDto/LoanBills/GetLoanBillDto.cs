using DPK.Mosavabat.Models.DataModels.Abrisham;
using System.Collections.Generic;

namespace DPK.Mosavabat.Models.EntitiesDto.LoanBills;

public record GetLoanBillDto(
    string ProgramName,
    string LoanId,
    string BillType,
    string Terminal,
    string TotalPhase,
    string Phase,
    ICollection<TransactionDto> Transactions,
    AbrishamResultDm Result,
    string CicsTransId,
    string SeqNo,
    string DataType,
    string Acknowledge,
    string Msgnbr);
