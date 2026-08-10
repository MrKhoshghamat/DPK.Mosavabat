namespace DPK.Mosavabat.Shared.DataModels.Mosavabat;

public class GetAccountBalanceAverageTarhJariZendegiDataModel
{
    public string CustomerNumber { get; set; }
    public int FacilityGroup { get; set; }
    public int FacilitySubTypeCode { get; set; }
    public string FromDate { get; set; }
    public int PeriodTime { get; set; }
}
