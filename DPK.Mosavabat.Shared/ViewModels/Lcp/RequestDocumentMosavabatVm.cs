namespace DPK.Mosavabat.Shared.ViewModels.Lcp
{
    public class RequestDocumentMosavabatVm
    {
        /// <summary>
        /// شناسه مورد
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// مبلغ اسمی سند
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// تاریخ سر رسید
        /// </summary>
        public string DueDate { get; set; }
        /// <summary>
        /// شماره سریال
        /// </summary>
        public string DocumentNumber { get; set; }
        /// <summary>
        /// نوع سند
        /// </summary>
        public string DocumentTypeCode { get; set; }
        /// <summary>
        /// مبلغ پرداختی
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// نام متعهد
        /// </summary>
        public string Owner { get; set; }
    }
}
