using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services;

public class ContractService : IContractService
{
    private readonly IRequestFacilityDatabase _requestFacilityDatabase;
    private readonly ILoggerManager _logger;

    public ContractService(ILoggerManager logger, IRequestFacilityDatabase requestFacilityDatabase)
    {
        _logger = logger;
        _requestFacilityDatabase = requestFacilityDatabase;
    }

    public async Task<string?> GetContractFileLink(GetMorabeheContractFileLinkDataModel getMorabeheContractFileLinkDataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(getMorabeheContractFileLinkDataModel));
        return await _requestFacilityDatabase.GetContractFileLink(getMorabeheContractFileLinkDataModel);
    }
}