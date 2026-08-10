using DPK.Mosavabat.Shared.DataModels.Lcp;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;

public interface ITicketServices
{
    Task<string> CreateTicket(TicketDataModel ticketDataModel);
}