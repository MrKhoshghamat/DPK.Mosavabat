using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp
{
    public interface ILcpServiceProxy
    {
        Task<string> AcceptAgent(AcceptAgentDataModel acceptAgent);
        Task<SendApprovalLetterDto> SendApprovalLetter(SendApprovalLetterVm approvalLetter, CancellationToken cancellationToken);
    }
}
