using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.DataModels.Lms
{
    public class InsertPaymentPermissionDataModel
    {
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
        /// شماره حساب
        /// </summary>
        public string AccNo { get; set; }

        /// <summary>
        /// کد نوع عقد
        /// </summary>
        public int ContractType { get; set; }

        /// <summary>
        /// کد زیر نوع عقد
        /// </summary>
        public int SubContractType { get; set; }

        /// <summary>
        /// مبلغ اصل
        /// </summary>
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// مبلغ پیش دریافت
        /// </summary>
        public decimal PrepaidAmount { get; set; }

        //[Required(ErrorMessage = "وارد کردن مبلغ پیش دریافت الزامی می باشد")]
        //public int? DetermineFacilityGroup { get; set; }
        public int? IssuancePaymentId { get; set; }

        public string? IssuancePaymentRadNumber { get; set; }
    }
}