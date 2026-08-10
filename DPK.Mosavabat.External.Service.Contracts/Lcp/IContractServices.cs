using DPK.Mosavabat.Shared.DataModels.Lcp;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;

public interface IContractServices
{
    Task<string?> PrintContract(InsertContractDataModel insertContractDataModel);
}