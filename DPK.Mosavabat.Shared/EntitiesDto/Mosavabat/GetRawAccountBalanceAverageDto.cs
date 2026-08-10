namespace DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

public class GetRawAccountBalanceAverageDto
{
    public string CustomerNo { get; set; }
    public string AccountNo { get; set; }
    public string AccountTitle { get; set; }
    public decimal Average3Months { get; set; }
    public decimal Average6Months { get; set; }
    public decimal Average9Months { get; set; }
    public decimal Average12Months { get; set; }
}