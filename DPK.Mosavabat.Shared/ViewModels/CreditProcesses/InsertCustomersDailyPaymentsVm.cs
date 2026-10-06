using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses;

public class InsertCustomersDailyPaymentsVm
{
    [Required(ErrorMessage = "کد شعبه الزامی است.")]
    [Range(1, int.MaxValue, ErrorMessage = "کد شعبه نامعتبر است.")]
    public int BranchCode { get; set; }

    [MaxLength(20, ErrorMessage = "کد منطقه حداکثر ۲۰ کاراکتر است.")]
    public string? RegionCode { get; set; }

    [Required(ErrorMessage = "شماره مشتری الزامی است.")]
    [MaxLength(30, ErrorMessage = "شماره مشتری حداکثر ۳۰ کاراکتر است.")]
    public string CustomerNo { get; set; } = string.Empty;

    public int CustomerType { get; set; }

    [MaxLength(50)]
    public string? Client { get; set; }

    [Required]
    public int ContractType { get; set; }

    [Required]
    public int SubContractType { get; set; }

    [Required(ErrorMessage = "شناسه C13 الزامی است.")]
    [MaxLength(50)]
    public string C13 { get; set; } = string.Empty;

    [Required(ErrorMessage = "شناسه C16 الزامی است.")]
    [MaxLength(50)]
    public string C16 { get; set; } = string.Empty;

    [Required(ErrorMessage = "مبلغ الزامی است.")]
    public long ContractAmount { get; set; }
}