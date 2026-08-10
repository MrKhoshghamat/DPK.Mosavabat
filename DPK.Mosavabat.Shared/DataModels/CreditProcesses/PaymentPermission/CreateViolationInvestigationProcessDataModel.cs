using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission
{
    [XmlRoot(ElementName = "Infraction")]
    public class CreateViolationInvestigationProcessDataModel
    {
        [XmlElement(ElementName = "CreateDate")]
        public DateTime CreateDate { get; set; }
        [XmlElement(ElementName = "CuApprovalSubject")]
        public int? CuApprovalSubject { get; set; }
        [XmlElement(ElementName = "ContractNo")]
        public string? ContractNo { get; set; }
        [XmlElement(ElementName = "Branches")]
        public int? Branches { get; set; }
        [XmlElement(ElementName = "InfractionType")]
        public int? InfractionTypeDataModel { get; set; }
        [XmlElement(ElementName = "Inspector")]
        public int? Inspector { get; set; }
    }
}

