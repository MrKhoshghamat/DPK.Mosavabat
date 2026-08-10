namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat
{
    public class GetRelatedAccountBalanceAverageVM
    {
        // شماره مشتری
        public string? CustomerNo { get; set; }
        // شماره حساب
        public string? AccountNumber { get; set; }
        // شناسه درخواست
        public int ApprovalRequestId { get; set; }
        // شناسه مصوبه
        public int CuMosavabeId { get; set; }
        // نوع بازپرداخت
        public int TypeOfContractLength { get; set; }
        // حداکثر بازه قرارداد
        public int MaxContractLength { get; set; }
        // کد زیر نوع سپهری
        public string? Code { get; set; }
        // ماه جاری
        public string? CurrentMonth { get; set; }
        // کد نوع عقد
        public int FacilityGroup { get; set; }
    }
}
