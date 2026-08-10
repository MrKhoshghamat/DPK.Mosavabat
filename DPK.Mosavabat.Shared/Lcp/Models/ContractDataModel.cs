using DPK.Mosavabat.Shared.DataModels.Lcp;

namespace DPK.Mosavabat.Shared.Lcp.Models;

public class ContractDataModel
{
    public int CaseId { get; set; }

    /// <summary>
    /// آی دی موضوع مصوبه
    /// </summary>
    public string CUApprovalSubjectId { get; set; }

    /// <summary>
    /// نوع عقد
    /// </summary>
    public int FacilityGroupCode { get; set; }

    /// <summary>
    /// لیست اسناد
    /// </summary>
    public List<RequestDocumentMosavabatVm> RequestDocuments { get; set; }

    /// <summary>
    /// رئیس شعبه
    /// </summary>
    public string BranchManager { get; set; }

    /// <summary>
    /// تاریخ تعهدنامه
    /// </summary>
    public DateTime ContractDate { get; set; }

    /// <summary>
    /// کد شعبه
    /// </summary>
    public string BranchCode { get; set; }

    /// <summary>
    /// شماره کارمندی رئیس شعبه یا جانشین آن
    /// </summary>
    public string EmployeeCode { get; set; }

    /// <summary>
    /// موضوع تسهیلات
    /// </summary>
    public string ContractSubject { get; set; }

    /// <summary>
    /// کل بهای فروش مورد معامله به متقاضی به عدد
    /// </summary>
    public long ContractAmount1Int { get; set; }

    /// <summary>
    /// قیمت تمام شده خرید مورد معامله توسط بانک به عدد
    /// </summary>
    public long ContractAmount2Int { get; set; }

    /// <summary>
    /// نرخ سود
    /// </summary>
    public string BenefitRate { get; set; }

    /// <summary>
    /// مبلغ سود به عدد
    /// </summary>
    public long BenefitAmountInt { get; set; }

    /// <summary>
    /// مبلغ پیش دریافت به حروف
    /// </summary>
    public long CustomerCashPriceInt { get; set; }

    /// <summary>
    /// مابقی مبلغ به عدد
    /// </summary>
    public long ContractAmount3Int { get; set; }

    /// <summary>
    /// نحوه بازپرداخت
    /// </summary>
    public string RefundWay { get; set; }

    /// <summary>
    /// مدت بازپرداخت
    /// </summary>
    public string FacilityLife { get; set; }

    /// <summary>
    /// تعداد اقساط
    /// </summary>
    public string CountOfInstallments { get; set; }

    /// <summary>
    /// مدت قرارداد
    /// </summary>
    public DateTime ContractDueDate { get; set; }

    /// <summary>
    /// سود متعلقه
    /// </summary>
    public string InterestRate { get; set; }

    /// <summary>
    /// لیست وثایق+درصد کاور
    /// </summary>
    public string AllDeposit { get; set; }

    /// <summary>
    /// این فیلد با مقدار ثابت "3" پر می شود
    /// </summary>
    public string CountOfContractVersion { get; set; }

    /// <summary>
    /// تاریخ تعهدنامه
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// نوع حساب
    /// </summary>
    public string AccountType { get; set; }

    /// <summary>
    /// شماره حساب
    /// </summary>
    public string AccountNo { get; set; }

    /// <summary>
    /// مبلغ وثیقه
    /// </summary>
    public long CollateralAmount { get; set; }

    /// <summary>
    /// شماره قرارداد
    /// </summary>
    public string ContractNo { get; set; }

    /// <summary>
    /// شماره معرفی نامه
    /// </summary>
    public string IntroductionLetterNo { get; set; }

    /// <summary>
    /// تاریخ معرفی نامه
    /// </summary>
    public DateTime IntroductionLetterDate { get; set; }

    /// <summary>
    /// مبلغ پرداختی سند
    /// </summary>
    public long PaymentAmountOfDocument { get; set; }

    /// <summary>
    ///جمع مبلغ اسمی سند
    /// </summary>
    public long DocumentAmountSum { get; set; }

    /// <summary>
    /// جمع مبلغ پرداختی سند
    /// </summary>
    public long PaymentAmountSum { get; set; }

    /// <summary>
    /// مبلغ اصل
    /// </summary>
    public long ContractAmount { get; set; }

    ///// <summary>
    ///// کل بهای فروش مورد معامله به متقاضی به حروف
    ///// </summary>
    ////public string ContractAmount1String { get; set; }
    ///// <summary>
    /////  قیمت تمام شده خرید مورد معامله توسط بانک به حروف
    ///// </summary>
    ////public string ContractAmount2String { get; set; }
    ///// <summary>
    ///// مبلغ سود به حروف
    ///// </summary>
    ////public string BenefitAmountString { get; set; }
    ///// <summary>
    ///// مبلغ پیش دریافت به حروف
    ///// </summary>
    ////public string CustomerCashPriceString { get; set; }
    ///// <summary>
    ///// مابقی مبلغ به حروف
    ///// </summary>
    ////public string ContractAmount3String { get; set; }
    ///// <summary>
    ///// بازه زمانی بازپرداخت
    ///// </summary>
    //// public string PaymentPeriod { get; set; }
    /////// <summary>
    /////// جمع مبلغ اسمی سند به حروف
    /////// </summary>
    ////public string DocumentAmountSumString { get; set; }
    ///// <summary>
    ///// جمع مبلغ پرداختی سند به حروف
    ///// </summary>
    //public string PaymentAmountSumString { get; set; }
}