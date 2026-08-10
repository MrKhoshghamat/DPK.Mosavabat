using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission
{
    public class InsertPaymentPermissionVm
    {
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
        public int ContractType { get; set; }

        /// <summary>
        /// کد زیر نوع عقد
        /// </summary>
        public int? SubContractType { get; set; }

        /// <summary>
        /// مبلغ اصل
        /// </summary>
        public long ContractAmount { get; set; }

        /// <summary>
        /// مبلغ پیش دریافت
        /// </summary>
        public long PrepaidAmount { get; set; }
    }
}
