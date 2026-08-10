namespace DPK.Mosavabat.Shared.Lcp.Models;

public class AgentAcceptDataModel
{
    /// <summary>
    ///     کد نماینده تایید شده
    /// </summary>
    public int? AgentId { get; set; } = null;

    /// <summary>
    ///     کد دریافتی از سامانه مصوبات
    /// </summary>
    public int CaseId { get; set; }

    /// <summary>
    ///     تایید شده
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    ///     پیام دریافتی
    /// </summary>
    public string Message { get; set; }
}