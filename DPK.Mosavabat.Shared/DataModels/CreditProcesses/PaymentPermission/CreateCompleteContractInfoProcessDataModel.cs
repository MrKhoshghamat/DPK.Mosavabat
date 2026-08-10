using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission
{
    [XmlRoot(ElementName = "LCP_Collaterals")]
    public class CreateCompleteContractInfoProcessDataModel
    {
        [XmlElement(ElementName = "Mab_Asl")]
        public decimal? MabAsl { get; set; }
        [XmlElement(ElementName = "CUApprovalSubject")]
        public int CuApprovalSubjectId { get; set; }
        [XmlElement(ElementName = "C10")]
        public string? C10 { get; set; }
        [XmlElement(ElementName = "C13")]
        public string? C13 { get; set; }
        [XmlElement(ElementName = "C16")]
        public string? C16 { get; set; }
        [XmlElement(ElementName = "Date_g")]
        public DateTime? Date_g { get; set; }
        [XmlElement(ElementName = "shob")]
        public string? Shob { get; set; }
        //[XmlElement(ElementName = "Creator")]
        //public int? Creator { get; set; }
    }
}