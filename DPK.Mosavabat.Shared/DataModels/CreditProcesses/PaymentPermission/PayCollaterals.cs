using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_Collaterals")]
public class PayCollaterals
{
    [XmlElement(ElementName = "PAY_Collateral")]
    public List<PayCollateral> Collaterals { get; set; }
}