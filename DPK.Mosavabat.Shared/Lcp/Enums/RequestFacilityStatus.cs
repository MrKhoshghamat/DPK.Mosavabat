namespace DPK.Mosavabat.Shared.Lcp.Enums;

public enum RequestFacilityStatus : short
{
    //[Display(Name = "ثبت درخواست")]
    BeginRegister = 1,

    //[Display(Name = "تکمیل درخواست")]
    RegisterCompleted = 5,

    //[Display(Name = "ارسال به بیزاجی")]
    SendToBizagi = 2,

    ReceiveFromBizagi = 10,

    /// <summary>
    ///     کسری مدارک
    /// </summary>
    //[Display(Name = "کسری مدارک")]
    DeficiencyOfDocuments = 50,

    /// <summary>
    ///     مدارک نامعتبر
    /// </summary>
    //[Display(Name = "مدارک نامعتبر")]
    InvalidDocuments = 60,
    EndRegister = 100
}