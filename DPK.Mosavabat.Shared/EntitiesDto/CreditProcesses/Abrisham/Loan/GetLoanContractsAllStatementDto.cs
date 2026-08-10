using DPK.Mosavabat.Shared.EntitiesDto.Abrisham.Loan;

namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;

public class GetLoanContractsAllStatementDto
{
    public string DataStatus { get; set; }

    public string Status { get; set; }

    public string ResponseCode { get; set; }

    public string ResponseMessageDescription { get; set; }

    public string CustomerNumber { get; set; }

    public string CustomerDescription { get; set; }

    public string BranchNo { get; set; }

    public string FacilityType { get; set; }

    public string BranchInfo { get; set; }

    public string InterestRate { get; set; }

    public string PenaltyRate { get; set; }

    public ICollection<TransactionInfoDto> Transactions { get; set; }

    public AbrishamResultDto Result { get; set; }

    public string CicsTransId { get; set; }

    public string SeqNo { get; set; }

    public string DataType { get; set; }

    public string Acknowledge { get; set; }

    public string Msgnbr { get; set; }
}