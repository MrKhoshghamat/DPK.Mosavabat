using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;

public interface IExternalServiceProvider
{
    Task<AcceptAgentDto> AcceptAgentService(AcceptAgentVm viewModel, CancellationToken cancellationToken);
    Task<UpdateRequestFacilityDto> AcceptRequestFacilityService(UpdateRequestFacilityVm viewModel, CancellationToken cancellationToken);
    Task<UpdateRequestFacilityRadNumberDto> UpdateRadNumberRequestFacilityService(UpdateRequestFacilityRadNumberVm viewModel, CancellationToken cancellationToken);
    Task<InsertContractDto?> PrintContract(InsertContractVm viewModel, CancellationToken cancellationToken);
    Task<TicketDto> CreateTicketService(TicketVm viewModel, CancellationToken cancellationToken);
    Task<ProcessIssuancePaymentDto> DiscussionPaymentPermission(DiscussionPaymentPermissionVm viewModel, CancellationToken cancellationToken);
   
}