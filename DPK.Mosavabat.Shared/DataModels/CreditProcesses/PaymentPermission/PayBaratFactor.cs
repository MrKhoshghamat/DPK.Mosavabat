using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_BaratFactor")]
public class PayBaratFactor
{
    [XmlElement(ElementName = "IssuancePeymentId")]
    public int IssuancePeymentId { get; set; }

    [XmlElement(ElementName = "FactorId")]
    public string FactorId { get; set; }

    [XmlElement(ElementName = "LcpId")]
    public int LcpId { get; set; }
}