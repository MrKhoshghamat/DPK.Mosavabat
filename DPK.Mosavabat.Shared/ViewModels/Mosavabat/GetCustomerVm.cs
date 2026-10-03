using DPK.Mosavabat.Common.Enums.Customer;

namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

public class GetCustomerVm
{
    /// <summary>
    /// شناسه ملی حقوقی
    /// </summary>
    public decimal? LegalNationalId { get; set; }
    /// <summary>
    /// کد ملی حقیقی
    /// </summary>
    public decimal? NationalCode { get; set; }
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public decimal? CustomerNumber { get; set; }
    /// <summary>
    /// نوع مشتری
    /// </summary>
    public CustomerType CustomerType { get; set; } = CustomerType.Legal;
    /// <summary>
    /// شماره مشتری
    /// </summary>
    public decimal AccountNumber { get; set; }

    /// <summary>
    /// فیلتر کردن حساب های غیر فعال
    /// </summary>
    public bool FilterAccount { get; set; } = true;
}