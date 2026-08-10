namespace DPK.Mosavabat.Shared.ViewModels.Lcp
{
    public class InsertContractVm
    {
        /// <summary>
        ///     شناسه مورد
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        ///     شناسه موضوع
        /// </summary>
        public int CuApprovalSubjectId { get; set; }

        /// <summary>
        ///     کد نوع عقد
        /// </summary>
        public int FacilityGroupCode { get; set; }

        /// <summary>
        ///     شماره قرارداد
        /// </summary>
        public string ContractNo { get; set; }

        /// <summary>
        ///     نام رئیس شعبه
        /// </summary>
        public string BrManagerName { get; set; }

        /// <summary>
        ///     کد پرسنلی رئیس شعبه
        /// </summary>
        public int EmployeeCode { get; set; }

        /// <summary>
        ///     تاریخ قرارداد
        /// </summary>
        public DateTime ContractDate { get; set; }

        /// <summary>
        ///     کد شعبه
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        ///     موضوع قرارداد
        /// </summary>
        public string ContractSubject { get; set; }

        /// <summary>
        ///     کل بهای فروش مورد معامله به متقاضی به عدد
        /// </summary>
        public long ContractAmount1Int { get; set; }

        /// <summary>
        ///     قیمت تمام شده خرید مورد معامله توسط بانک به عدد
        /// </summary>
        public long ContractAmount2Int { get; set; }

        /// <summary>
        ///     نرخ سود
        /// </summary>
        public string BenefitRate { get; set; }

        /// <summary>
        ///     مبلغ سود قرارداد
        /// </summary>
        public long BenefitAmountInt { get; set; }

        /// <summary>
        ///     مبلغ پیش پرداخت
        /// </summary>
        public long CustomerCashPriceInt { get; set; }

        /// <summary>
        ///     مابقی مبلغ
        /// </summary>
        public long ContractAmount3Int { get; set; }

        /// <summary>
        ///     نحوه بازپرداخت
        /// </summary>
        public string RefundWay { get; set; }

        /// <summary>
        ///     دوره های اقساط
        /// </summary>
        public string FacilityLife { get; set; }

        /// <summary>
        ///     تعداد اقساط
        /// </summary>
        public int CountOfInstallments { get; set; }

        /// <summary>
        ///     مدت قرارداد
        /// </summary>
        public DateTime ContractDueDate { get; set; }

        /// <summary>
        ///     نرخ وجهه التزام
        /// </summary>
        public string InterestRate { get; set; }

        /// <summary>
        ///     لیست وثایق
        /// </summary>
        public string AllDeposit { get; set; }

        /// <summary>
        /// نوع سند قرارداد
        /// </summary>
        public int ContractTemplateCode { get; set; }

        /// <summary>
        ///     تعداد ورژن
        /// </summary>
        public string CountOfContractVersion { get; set; }

        /// <summary>
        ///     تاریخ قرارداد
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        ///     نوع حساب
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        ///     شماره حساب
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        ///     مبلغ وثیقه
        /// </summary>
        public long CollateralAmount { get; set; }

        /// <summary>
        /// شماره معرفی نامه
        /// </summary>
        public string IntroductionLetterNo { get; set; }

        /// <summary>
        /// تاریخ معرفی نامه
        /// </summary>
        public DateTime IntroductionLetterDate { get; set; }
        //public string PaymentAmountOfDocument { get; set; }

        /// <summary>
        /// جمع مبلغ اسناد
        /// </summary>
        public string DocumentAmountSum { get; set; }

        /// <summary>
        /// جمع مبلغ پرداختی سند
        /// </summary>
        public string PaymentAmountSum { get; set; }

        /// <summary>
        /// مبلغ اصل
        /// </summary>
        public string ContractAmount { get; set; }

        /// <summary>
        /// لیست اسناد
        /// </summary>
        public List<DataModels.Lcp.RequestDocumentMosavabatVm> ContractDocuments { get; set; }
    }
}
