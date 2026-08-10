namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission
{
    public class PaymentPermissionDataModel
    {
        public int CollateralNo { get; set; }
        public int ContractAmount { get; set; }
        public int CustomerShareAmount { get; set; }
        public int BranchCode { get; set; }
        public int CustomerNo { get; set; }
        public int ContractTypeId { get; set; }
        public int FacilitySubTypeCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}