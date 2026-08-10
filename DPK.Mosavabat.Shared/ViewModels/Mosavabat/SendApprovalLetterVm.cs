using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

public class SendApprovalLetterVm
{
    #region کدهای مرجع BaseInfo / دیتابیس عقود

    public int CaseId { get; set; }

    /// <summary>گروه تسهیلات و تعهدات - BaseInfoCategoryIds.FacilityGroup</summary>
    [Display(Name = "گروه تسهیلات و تعهدات")]
    public int FacilityGroup { get; set; }

    /// <summary>نوع عقد - Lcp.ContractTypes</summary>
    [Display(Name = "نوع عقد")]
    public int ContractType { get; set; }

    /// <summary>زیرنوع عقد - BaseInfoCategoryIds.SubFacilityGroup</summary>
    [Display(Name = "زیرنوع عقد")]
    public List<SubFacilityGroupInfoVm> SubFacilityGroups { get; set; } = new List<SubFacilityGroupInfoVm>();

    /// <summary>نوع بازپرداخت - BaseInfoCategoryIds.RefundType</summary>
    [Display(Name = "نوع بازپرداخت")]
    public int RefundType { get; set; }

    /// <summary>نحوه بازپرداخت - BaseInfoCategoryIds.RefundWay</summary>
    [Display(Name = "نحوه بازپرداخت")]
    public int? RefundWay { get; set; }

    /// <summary>نوع سازمان ذینفع - BaseInfoCategoryIds.StackHolderType</summary>
    [Display(Name = "نوع سازمان ذینفع")]
    public int? StackHolderType { get; set; }

    /// <summary>نوع تخصیص اعتبار (نحوه مصرف) -
    /// TODO: نیاز به تعریف CategoryId در BaseInfo</summary>
    [Display(Name = "نوع تخصیص اعتبار (نحوه مصرف)")]
    public int? SubjectType { get; set; }

    /// <summary>نوع درخواست -
    /// TODO: نیاز به تعریف CategoryId در BaseInfo</summary>
    [Display(Name = "نوع درخواست")]
    public int? RequestTypeDetail { get; set; }

    /// <summary>نوع گروهی -
    /// TODO: نیاز به تعریف CategoryId در BaseInfo</summary>
    [Display(Name = "نوع گروهی")]
    public int? GroupType { get; set; }

    /// <summary>نوع ارز - عدم تبادل</summary>
    [Display(Name = "نوع ارز")]
    public int? CurrencyType { get; set; }

    #endregion

    #region مدت، مبالغ و نرخ ها

    [Display(Name = "مدت قرارداد")]
    public int? ContractPeriod { get; set; }

    [Display(Name = "مبلغ قرارداد")]
    public decimal? ContractAmount { get; set; }

    [Display(Name = "فاصله زمانی اقساط")]
    public int? InstallmentInterval { get; set; }

    /// <summary>در صورتی که تسهیلات با مدت تنفس تصویب شده باشد باید نمایش داده شود</summary>
    [Display(Name = "مدت تنفس")]
    public int? BreathingTime { get; set; }

    [Display(Name = "مبلغ تسهیلات بانک")]
    public decimal? BankSharePrice { get; set; }

    [Display(Name = "مبلغ پیش دریافت")]
    public decimal? PreReceived { get; set; }

    [Display(Name = "نرخ کارمزد")]
    public string FeeRate { get; set; }

    [Display(Name = "نرخ وجه التزام")]
    public string PenaltyRate { get; set; }

    [Display(Name = "نرخ سود عهده مشتری")]
    public string CustomerInterestRate { get; set; }

    [Display(Name = "نرخ سود")]
    public string InterestRate { get; set; }

    [Display(Name = "سپرده نقدی")]
    public decimal? CashDepositAmount { get; set; }

    [Display(Name = "مبلغ غیر نقدی ضمانت نامه")]
    public decimal? WarrantyNonCashAmount { get; set; }

    [Display(Name = "مبلغ گشایش اعتبار")]
    public decimal? CreditOpeningAmount { get; set; }

    [Display(Name = "مدت مشارکت")]
    public int? PartnershipPeriod { get; set; }

    [Display(Name = "سرمایه کل مشارکت")]
    public decimal? TotalPartnershipCapital { get; set; }

    [Display(Name = "سهم الشرکه نقدی مشتری")]
    public decimal? CustomerCashAmount { get; set; }

    [Display(Name = "سهم الشرکه غیر نقدی مشتری")]
    public decimal? CustomerNonCashAmount { get; set; }

    [Display(Name = "سقف مبلغ فردی (گروهی)")]
    public decimal? IndividualAmountCeiling { get; set; }

    #endregion

    #region اطلاعات متقاضی، وثیقه و شروط

    [Display(Name = "انواع وثیقه ها و درصد پوشش آن")]
    public List<CollateralItemVm> Collaterals { get; set; }

    [Display(Name = "شروط مصوبه")]
    public List<ApprovalConditionItemVm>? ApprovalConditions { get; set; }

    #endregion

    #region شعبه و تاریخ ها

    [Display(Name = "تاریخ انقضای مصوبه")]
    public DateTime? ApprovalExpireDate { get; set; }

    [Display(Name = "نام شعبه")]
    public string BranchName { get; set; }

    [Display(Name = "کد شعبه")]
    public int BranchCode { get; set; }

    #endregion

    #region وضعیت ها و پرچم ها (Flags)

    /// <summary>در صورتی که true باشد، مصوبه ابطال شده است</summary>
    [Display(Name = "وضعیت ابطال بودن مصوبه")]
    public bool InValid { get; set; }

    [Display(Name = "وضعیت فعال بودن مصوبه")]
    public bool IsBlock { get; set; }

    /// <summary>در صورتی که گروهی باشد، باید لیست افراد گروه نیز دریافت شود</summary>
    [Display(Name = "گروهی")]
    public bool IsGroup { get; set; }

    [Display(Name = "ترکیبی")]
    public bool? IsTarkibi { get; set; }

    [Display(Name = "تعداد مصوبات انفرادی (گروهی)")]
    public int? IndividualApprovalsCount { get; set; }

    #endregion

    #region ضامنین

    /// <summary>تعداد ضامنین با رنک</summary>
    public IEnumerable<GuarantorDataModelVm> GuarantorDataModels { get; set; }

    /// <summary>کد ملی ضامن که اجباری نمی باشد</summary>
    
    public string? GuarantorNationalCode { get; set; }

    /// <summary>توضیحات گارانتی کننده</summary>
    public string? GuarantorDescription { get; set; }

    #endregion
}