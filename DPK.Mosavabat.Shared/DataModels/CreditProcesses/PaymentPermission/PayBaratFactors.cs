using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_BaratFactors")]
public class PayBaratFactors
{
    [XmlElement(ElementName = "PAY_BaratFactor")]
    public List<PayBaratFactor> Factors { get; set; }
}