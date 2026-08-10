using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DPK.Mosavabat.Shared.DataModels.Lms
{
    public class NumericOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !int.TryParse(value.ToString(), out _))
            {
                return new ValidationResult("شماره مشتری حتما باید به صورت عدد باشد");
            }

            return ValidationResult.Success;
        }
    }
    public class CheckAndUpdateC13InPaymentPermissionDataModel
    {
        
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
        
        public long ContractPrepaidAmountsSum { get; set; }

        /// <summary>
        /// شماره قرارداد 13 رقمی
        /// </summary>
        
        public long C13 { get; set; }

    }
}