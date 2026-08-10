using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

[XmlRoot(ElementName = "IssuancePeymentId")]
public class IssuancePaymentPermissionDataModel
{
    [XmlElement(ElementName = "GUID")] public string GUID { get; set; }
    [XmlElement(ElementName = "Creator")] public int Creator { get; set; }
    [XmlElement(ElementName = "CUApprovalSubject")] public int CUApprovalSubject { get; set; }
    [XmlElement(ElementName = "Request_Date")] public DateTime Request_Date { get; set; }
    [XmlElement(ElementName = "CU_Customer")] public int CU_Customer { get; set; }
    [XmlElement(ElementName = "Branches")] public int Branches { get; set; }
    [XmlElement(ElementName = "ContractAmount")] public long ContractAmount { get; set; }
    [XmlElement(ElementName = "CustomerCashPrice")] public long CustomerCashPrice { get; set; }
    [XmlElement(ElementName = "BankSharePrice")] public long BankSharePrice { get; set; }
    [XmlElement(ElementName = "CustomerNonCashPrice")] public long CustomerNonCashPrice { get; set; }
    [XmlElement(ElementName = "BaratDueDate")] public DateTime BaratDueDate { get; set; }
    [XmlElement(ElementName = "Description")] public string? Description { get; set; }
    [XmlElement(ElementName = "FeeAmount")] public long FeeAmount { get; set; }
    [XmlElement(ElementName = "PAY_Guarantors")] public PayGuarantors Guarantors { get; set; }
    [XmlElement(ElementName = "PAY_Collaterals")] public PayCollaterals Collaterals { get; set; }
    [XmlElement(ElementName = "PAY_BaratFactors")] public PayBaratFactors Factors { get; set; }
    [XmlElement(ElementName = "PAY_BaratBeneficiaries")] public PayBaratBeneficiaries Beneficiaries { get; set; }
    [XmlElement(ElementName = "LcpId")] public string LcpId { get; set; }
    [XmlElement(ElementName = "CreditRank")] public string? CreditRank { get; set; }
    [XmlElement(ElementName = "CreditScore")] public float? CreditScore { get; set; }
}
