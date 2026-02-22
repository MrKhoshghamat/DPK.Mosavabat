namespace DPK.Mosavabat.Models.DataModels.Contracts;

public class GetSubContractTypeTitleBySubContractTypeIdDm(
    int subContractTypeId)
{
    public int SubContractTypeId { get; } = subContractTypeId;
}