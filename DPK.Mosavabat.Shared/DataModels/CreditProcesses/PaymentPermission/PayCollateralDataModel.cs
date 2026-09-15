using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

public class PayCollateralDataModel
{
    public string Amount { get; set; }
    public int CollateralType { get; set; }
    public string Description { get; set; }
    public int LcpId { get; set; }
    public decimal CoverPercent { get; set; }
}
