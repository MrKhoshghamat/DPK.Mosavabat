using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.Lcp;
using Newtonsoft.Json;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;

namespace DPK.Mosavabat.External.Services.Lcp;

public class TicketServices : ITicketServices
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggerManager _logger;
    public TicketServices(IServiceProvider serviceProvider, ILoggerManager logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<string> CreateTicket(TicketDataModel ticketDataModel)
    {
        var response =  await _serviceProvider.CreateTicketService(ticketDataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response;
    }
}