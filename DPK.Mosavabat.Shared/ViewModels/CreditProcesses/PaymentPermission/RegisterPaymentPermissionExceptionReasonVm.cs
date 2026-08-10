using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission
{
    public class RegisterPaymentPermissionExceptionReasonVm
    {
        public int BranchCode { get; set; }
        public string NationalCode { get; set; }
        public string CustomerNumber { get; set; }
        public int IssuancePaymentPermissionId { get; set; }
        public int IssuanceLocation { get; set; }
        public string ExceptionCode { get; set; }
        public string LetterNumber { get; set; }
        public DateTime LetterDate { get; set; }
        public string RadNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public long Amount { get; set; }
    }
}
