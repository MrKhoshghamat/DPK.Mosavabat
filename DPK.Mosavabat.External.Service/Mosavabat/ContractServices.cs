using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Mosavabat;

namespace DPK.Mosavabat.External.Services.Mosavabat;

public class ContractServices : IContractServices
{
    private readonly ILoggerManager _logger;

    //private readonly ServiceProvider _serviceProvider;
    public ContractServices(ILoggerManager logger)
    {
        _logger = logger;
        //_serviceProvider = serviceProvider;
    }

    public string? GetContractFileLink(int contractId)
    {
        _logger.Error("External Service Test");

        return null;
    }
}