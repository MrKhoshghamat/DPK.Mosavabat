namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission
{
    public class CheckAndUpdateC13InPaymentPermissionVm
    {
        /// <summary>
        /// شماره مشتری
        /// </summary>
        public string CustomerNo { get; set; }

        /// <summary>
        /// کد شعبه
        /// </summary>
        public int BranchCode { get; set; }

        /// <summary>
        /// کد نوع عقد
        /// </summary>
        public int ContractType { get; set; }

        /// <summary>
        /// کد زیر نوع عقد
        /// </summary>
        public int SubContractType { get; set; }

        /// <summary>
        /// مبلغ قرارداد
        /// </summary>
        public long ContractAmount { get; set; }

        /// <summary>
        /// مبلغ پیش دریافت
        /// </summary>
        public long? PrepaidAmount { get; set; }

        /// <summary>
        /// شماره قرارداد 13 رقمی
        /// </summary>
        public long C13 { get; set; }
    }
}
