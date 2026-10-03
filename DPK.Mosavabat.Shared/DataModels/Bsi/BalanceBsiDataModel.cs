using DPK.Mosavabat.Shared.EntitiesDto.Lcp;

namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class BalanceBsiDataModel
{
    public decimal AccountNumber { get; set; }
    public decimal CustomerNumber { get; set; }
    public int BranchCode { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public CommercialFlag CommercialFlag { get; set; }

    public bool IsActive()
    {
        return BranchCode > 0
               && CurrencyType.Id == 0
               && AccountStatus.Id == 2;
    }
}