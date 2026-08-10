namespace DPK.Mosavabat.Shared.Lcp.Models;

public class UpdateRequestFacilityStatusDataModel
{
    /// <summary>
    ///     کد مصوبات
    /// </summary>
    public int? CaseId { get; set; }

    /// <summary>
    ///     مراحل درخواست
    /// </summary>
    public string RequestFacilityStatus { get; set; }

    /// <summary>
    ///     پیام
    /// </summary>
    public string Message { get; set; }
}