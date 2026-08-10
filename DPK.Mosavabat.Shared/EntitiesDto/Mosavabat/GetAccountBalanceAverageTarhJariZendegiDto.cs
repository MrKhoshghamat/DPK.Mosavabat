namespace DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

public class GetAccountBalanceAverageTarhJariZendegiDto
{
    public string CustomerNumber { get; set; }
    public string AccountNumber { get; set; }
    public int PeriodTime { get; set; }
    public float Coefficient { get; set; }
    public long Amount { get; set; }
    public long MaxGrant { get; set; }
    public string? AccountTitle { get; set; }
    public int Year { get; set; }
    public int Version { get; set; }
}
