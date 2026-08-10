using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_BaratBeneficiaries")]
public class PayBaratBeneficiaries
{
    [XmlElement(ElementName = "PAY_BaratBeneficiary")]
    public List<PayBaratBeneficiary> Beneficiaries { get; set; }
}