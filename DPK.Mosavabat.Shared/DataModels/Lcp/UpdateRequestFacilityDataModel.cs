namespace DPK.Mosavabat.Shared.DataModels.Lcp;

public class UpdateRequestFacilityDataModel
{
    /// <summary>
    ///     شناسه مورد
    /// </summary>
    public int CaseId { get; set; }

    /// <summary>
    ///     وضعیت درخواست تسهیلات اولیه
    /// </summary>
    public string? RequestFacilityStatus { get; set; }

    /// <summary>
    ///     توضیحات
    /// </summary>
    public string? Message { get; set; }
}