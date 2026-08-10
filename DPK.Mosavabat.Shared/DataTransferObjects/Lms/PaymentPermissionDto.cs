using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Shared.DataTransferObjects.Lms
{
    public class PaymentPermissionDto
    {
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
        public bool IsCanceled { get; set; }

        /// <summary>
        /// مبلغ اصل
        /// </summary>
        public long ContractAmount { get; set; }

        /// <summary>
        /// مبلغ پیش دریافت
        /// </summary>
        public long? PrepaidAmount { get; set; }
    }
}
