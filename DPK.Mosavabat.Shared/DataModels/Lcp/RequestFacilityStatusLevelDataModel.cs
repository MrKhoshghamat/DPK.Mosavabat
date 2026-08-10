using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Shared.DataModels.Lcp
{
    public class RequestFacilityStatusLevelDataModel 
    {

        /// <summary>
        /// آی دی لاگ در  مصوبات
        /// </summary>
        public int IdStatusLog { get; set; }
        /// <summary>
        /// تاریخ ورود به کارتابل
        /// </summary>
        public DateTime EntryDate { get; set; }
        /// <summary>
        /// تاریخ خروج از کارتابل
        /// </summary>
        public DateTime ExitDate { get; set; }
        /// <summary>
        /// نام و کد کارمند
        /// </summary>
        public string? CurrentEmployee { get; set; }
        /// <summary>
        /// وضعیت
        /// </summary>
        public string? Activity { get; set; }
        /// <summary>
        /// موقعیت
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string? StatusDescription { get; set; }
    }
}
