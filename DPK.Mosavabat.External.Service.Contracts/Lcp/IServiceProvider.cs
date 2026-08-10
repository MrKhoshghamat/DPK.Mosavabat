using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;
public interface IServiceProvider
{
    Task<string> AcceptAgentService(AcceptAgentDataModel acceptAgent);
    Task<string> AcceptRequestFacilityService(UpdateRequestFacilityDataModel updateRequestFacilityDataModel);
    Task<string> UpdateRadNumberRequestFacilityService(UpdateRequestFacilityRadNumber updateRequestFacilityRadNumber);
    Task<string?> PrintContract(InsertContractDataModel insertContractDataModel);
    Task<string> CreateTicketService(TicketDataModel ticketDataModel);
    Task<string> GetBoardMembersListService(GetBoardMemberDataModel getBoardMemberDataModel);
}