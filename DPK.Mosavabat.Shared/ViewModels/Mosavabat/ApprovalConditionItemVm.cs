using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

/// <summary>شروط مصوبه  </summary>
public class ApprovalConditionItemVm
{
    [Display(Name = "کد")]
    public int Code { get; set; }

    /// <summary>
    /// عنوان شرط، مثال: حفظ معدل موجودی در زمان پرداخت،
    /// اصلاح ساختار مالی شرکت، ارائه مستندات تکمیلی،
    /// عدم تجاوز سقف خالص مانده، حفظ/رعایت نسبت مالکانه و ...
    /// </summary>
    [Display(Name = "عنوان شرط")]
    public string Title { get; set; }
}