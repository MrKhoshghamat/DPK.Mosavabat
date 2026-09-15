namespace DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager
{
    public class MosavabatLegalRankingDataModel
    {
        /// <summary>
        /// رتبه اعتباری ایرانیان
        /// </summary>
        public ModavabatRankingDataModel IranianRanking { get; set; }
        /// <summary>
        /// رتبه اعتباری lcp
        /// </summary>
        public ModavabatRankingDataModel LocalRanking { get; set; }
        /// <summary>
        /// رتبه اعتباری شرکت خارجی
        /// </summary>
        public MosavabatExternalRankingDataModel ExternalRanking { get; set; }
    }
}
