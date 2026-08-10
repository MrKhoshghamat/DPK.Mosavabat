namespace DPK.Mosavabat.Shared.EntitiesDto.Mosavabat
{
    public class GetAccountBalanceAverageDto
    {
        public string? CustomerNo { get; set; }
        public string? AccountNo { get; set; }
        public string? PeriodTime { get; set; }
        public string? Coefficient { get; set; }
        public string? Amount { get; set; }
        public string? MaxGrant { get; set; }
        public string? AccountTitle { get; set; }
        public int Year { get; set; }
        public int Version { get; set; }
    }
}
