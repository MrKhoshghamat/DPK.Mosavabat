using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Common.Enums.Customer
{
    /// <summary>
    /// نوع شخص
    /// </summary>
    public enum CustomerType : byte
    {
        /// <summary>
        /// حقیقی
        /// </summary>
        [Display(Name = "حقیقی")]
        Real = 01,
        /// <summary>
        ///حقوقی
        /// </summary>
        [Display(Name = "حقوقی")]
        Legal = 02,
        /// <summary>
        /// اتباع
        /// </summary>
        [Display(Name = "اتباع")]
        Foreigners = 5,
    }
}
