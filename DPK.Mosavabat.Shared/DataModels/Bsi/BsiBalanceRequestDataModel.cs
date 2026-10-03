namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class BsiBalanceRequestDataModel
{
    public decimal? AccountNumber { get; set; }
    public decimal? CustomerNumber { get; set; }
    public bool FilterAccount { get; set; } = true;
}