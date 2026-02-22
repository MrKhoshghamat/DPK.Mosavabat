namespace DPK.Mosavabat.Models.DataModels.Contracts;

public class GetContractTypeTitleByContractTypeIdDm(
    int contractTypeId)
{
    public int ContractTypeId { get; } = contractTypeId;
}