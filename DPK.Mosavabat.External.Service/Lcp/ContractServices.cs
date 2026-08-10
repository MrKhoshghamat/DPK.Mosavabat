using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Newtonsoft.Json;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;

namespace DPK.Mosavabat.External.Services.Lcp;

public class ContractServices : IContractServices
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggerManager _logger;
    public ContractServices(IServiceProvider serviceProvider, ILoggerManager logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<string?> PrintContract(InsertContractDataModel insertContractDataModel)
    {
        var response = await _serviceProvider.PrintContract(insertContractDataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }
}