namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission
{
    public class PaymentPermissionIssuanceControlVm
    {
        public string MosavabeNo { get; set; }
        public long ContractAmount { get; set; }
        public long? CustomerSharePrice { get; set; }
        public int? SubContractType { get; set; }
        public string CustomerNo { get; set; }
    }
}
