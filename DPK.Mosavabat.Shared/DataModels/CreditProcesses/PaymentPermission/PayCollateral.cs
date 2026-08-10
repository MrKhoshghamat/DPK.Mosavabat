using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "PAY_Collateral")]
public class PayCollateral
{
    [XmlElement(ElementName = "IssuancePeymentId")]
    public int IssuancePeymentId { get; set; }

    [XmlElement(ElementName = "IsDeleted")]
    public bool IsDeleted { get; set; }

    [XmlElement(ElementName = "Amount")]
    public string Amount { get; set; }

    [XmlElement(ElementName = "CollateralType")]
    public int CollateralType { get; set; }

    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "LcpId")]
    public string LcpId { get; set; }

    [XmlElement(ElementName = "CoverPercent")]
    public decimal CoverPercent { get; set; }
}