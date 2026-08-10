namespace DPK.Mosavabat.Entities.Models
{
    public class PaymentPermission
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// شماره مصوبه
        /// </summary>
        public string MosavabeNo { get; set; }

        /// <summary>
        /// کد شعبه
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        /// شماره مشتری
        /// </summary>
        public string CustomerNo { get; set; }

        /// <summary>
        /// کد نوع عقد
        /// </summary>
        public string ContractType { get; set; }

        /// <summary>
        /// کد زیر نوع عقد
        /// </summary>
        public string SubContractType { get; set; }

        /// <summary>
        /// شماره قرارداد 13 رقمی
        /// </summary>
        public string? C13 { get; set; }

        /// <summary>
        /// تاریخ اعتبار
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// وضعیت ابطال
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        /// وضعیت حذف
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// مبلغ اصل
        /// </summary>
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// مبلغ پیش دریافت
        /// </summary>
        public decimal? PrepaidAmount { get; set; }

        /// <summary>
        /// تاریخ ساخت
        /// </summary>
        public DateTime CreationDate { get; set; }

    }
}
