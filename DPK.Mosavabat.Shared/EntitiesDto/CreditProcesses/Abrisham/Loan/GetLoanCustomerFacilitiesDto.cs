namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;

public class GetLoanCustomerFacilitiesDto
{
    public string CustomerDesc { get; set; }

    public string TotalDirectCommitment { get; set; }

    public string TotalIndirectCommitment { get; set; }

    public string BankTitle { get; set; }

    public ICollection<FacilityDto> FacilityList { get; set; }

    public string CicsTransId { get; set; }

    public string SeqNo { get; set; }

    public string DataType { get; set; }

    public string Acknowledge { get; set; }

    public string Msgnbr { get; set; }
}