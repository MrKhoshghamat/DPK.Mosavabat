using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_Guarantors")]
public class PayGuarantors
{
    [XmlElement(ElementName = "PAY_Guarantor")]
    public List<PayGuarantor> Guarantors { get; set; }
}