namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission
{
    public class PaymentPermissionDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string MosavabeNo { get; set; }

        public string BranchCode { get; set; }

        public string CustomerNo { get; set; }

        public string ContractType { get; set; }

        public string SubContractType { get; set; }

        public string C13 { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsCancel { get; set; }

        public bool IsDelete { get; set; }

        public long ContractAmount { get; set; }

        public long PrepaidAmount { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
