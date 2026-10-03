using DPK.Mosavabat.Common.Enums.Customer;

namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class BsiCustomerDataModel
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
    public string Token { get; set; }
}