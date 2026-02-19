namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class GetCollateralTypeIdDm(
    int collateralTypeCode)
{
    public int CollateralTypeCode { get; } = collateralTypeCode;
}