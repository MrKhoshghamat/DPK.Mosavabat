using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

/// <summary>
/// زیرنوع عقد. نام کلاس عمداً «Info» گرفته شده تا با نام پراپرتی مربوطه
/// در FacilityContract (SubFacilityGroups) تداخل نداشته باشد.
/// </summary>
public class SubFacilityGroupInfoVm
{
    [Display(Name = "کد")]
    public int Code { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }
}