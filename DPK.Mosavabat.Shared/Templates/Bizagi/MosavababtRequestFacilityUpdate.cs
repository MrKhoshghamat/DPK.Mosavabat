using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi;

[XmlRoot(ElementName = "LCP_ApprovelStatuse")]
public class MosavabatApproveStatusDataModel
{
    [XmlElement(ElementName = "ApproveStatuseId")]
    public int ApproveStatusId { get; set; }
}

[XmlRoot(ElementName = "LCP_RequestFacility")]
public class RequestFacilityDataModelUpdate : MosavabatRequestFacilityDataModel
{
    [XmlElement(ElementName = "LCP_ApprovelStatuse")]
    public MosavabatApproveStatusDataModel ApproveStatus { get; set; }
}