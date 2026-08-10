using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Services.Contracts;

public interface IContractService
{
    Task<string?> GetContractFileLink(GetMorabeheContractFileLinkDataModel getMorabeheContractFileLinkDataModel);
}