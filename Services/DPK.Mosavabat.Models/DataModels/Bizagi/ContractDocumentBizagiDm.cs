using System.Xml.Serialization;

namespace DPK.Mosavabat.Models.DataModels.Bizagi;

[XmlRoot(ElementName = "LCP_ContractDocument")]
public class ContractDocumentBizagiDm
{
    [XmlElement(ElementName = "LcpId")]
    public long LcpId { get; set; }

    [XmlElement(ElementName = "LcpRequestDocumentId")]
    public long Id { get; set; }

    [XmlElement(ElementName = "DocumentAmount")]
    public string Amount { get; set; }

    [XmlElement(ElementName = "DueDate")]
    public string DueDate { get; set; }

    [XmlElement(ElementName = "SerialNumber")]
    public string DocumentNumber { get; set; }

    [XmlElement(ElementName = "DocumentType")]
    public string? DocumentType { get; set; }

    [XmlElement(ElementName = "PaymentAmount")]
    public string PayAmount { get; set; }

    [XmlElement(ElementName = "CommittedBankName")]
    public string Owner { get; set; }
}