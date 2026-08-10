namespace DPK.Mosavabat.Shared.ResponseModels.CreditProcesses
{
    public class GetListOfPaymentPermissionResponseModel
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

        public decimal ContractAmount { get; set; }

        public decimal PrepaidAmount { get; set; }

        public DateTime CreationDate { get; set; }
    }
}