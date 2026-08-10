namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat
{
    public class GetAccountBalanceAverageVm
    {
        public string? CustomerNo { get; set; }
        public int ApprovalRequestId { get; set; }
        public int CuMosavabeId { get; set; }
        public int TypeOfContractLength { get; set; }
        public int MaxContractLength { get; set; }
        public string? Code { get; set; }
        public string? CurrentMonth { get; set; }
        public int FacilityGroup { get; set; }
    }
}
