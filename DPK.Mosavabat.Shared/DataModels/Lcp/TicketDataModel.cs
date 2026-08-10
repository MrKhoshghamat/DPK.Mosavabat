namespace DPK.Mosavabat.Shared.DataModels.Lcp
{
    public class TicketDataModel
    {
        public bool HasSubjectFacilityId { get; set; } = false;

        /// <summary>
        /// آی دی موضوع درخواست
        /// </summary>
        public int? SubjectFacilityId { get; set; }

        /// <summary>
        /// آی دی موضوع مصوبه
        /// </summary>
        public string CUApprovalSubjectId { get; set; }
        
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// مراحل درخواست
        /// </summary>
        public string RequestFacilityStatus { get; set; }
        
        /// <summary>
        /// شماره
        /// </summary>
        public long Number { get; set; }

        ///// <summary>
        ///// شماره تسک بیزاجی برای آپدیت
        ///// </summary>
        public string TaskId { get; set; }

        /// <summary>
        ///شناسه درخواست
        /// </summary>
        public int RequestFacilityId { get; set; }
    }
}



