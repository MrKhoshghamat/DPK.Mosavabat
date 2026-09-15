namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    public class MosavabatExternalRankingDataModel
    {
        /// <summary>
        /// فایل پی دی اف
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// رتبه کوتاه مدت اختصاص یافته به متقاضی اعتبار بانکی
        /// </summary>
        public string ShortTermRateSymbol { get; set; }
        /// <summary>
        /// رتبه بلند مدت اختصاص یافته به متقاضی اعتبار بانکی
        /// </summary>
        public string LongTermRateSymbol { get; set; }

        /// <summary>
        /// تاریخ انتقضای رتبه
        /// </summary>
        public string RateExpireDate { get; set; }
        /// <summary>
        /// تاریخ اختصاص رتبه
        /// </summary>
        public string RateAssignDate { get; set; }
        /// <summary>
        /// امتیاز رتبه اختصاص یافته در مقیاس 0 تا 100
        /// </summary>
        public int? Score { get; set; }
    }
}
