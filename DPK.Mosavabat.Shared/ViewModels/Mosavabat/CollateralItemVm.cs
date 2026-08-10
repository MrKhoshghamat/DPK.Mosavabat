using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

/// <summary>انواع وثیقه ها و درصد پوشش آن ها - CUDeposits</summary>
public class CollateralItemVm
{
    [Display(Name = "کد گروه وثیقه")]
    public int DepositGroupCode { get; set; }

    [Display(Name = "عنوان گروه وثیقه")]
    public string DepositGroupTitle { get; set; }

    [Display(Name = "کد نوع وثیقه")]
    public int DepositTypeCode { get; set; }

    [Display(Name = "نوع وثیقه")]
    public string DepositType { get; set; }

    [Display(Name = "درصد پوشش")]
    public int CoverPercent { get; set; }
}