using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.DataModels.Lms
{
    public class PaymentPermissionIssuanceControlDataModel
    {
        public string MosavabeNumber { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal CustomerSharePrice { get; set; }
        public int SubContractType { get; set; }
        public string CustomerNo { get; set; }
        //public int? DetermineFacilityGroup { get; set; }
    }
}
