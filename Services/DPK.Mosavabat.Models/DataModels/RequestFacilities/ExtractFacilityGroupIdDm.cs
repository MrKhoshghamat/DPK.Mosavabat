namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class ExtractFacilityGroupIdDm(
    string facilityGroupCode)
{
    public string FacilityGroupCode { get; } = facilityGroupCode;
}