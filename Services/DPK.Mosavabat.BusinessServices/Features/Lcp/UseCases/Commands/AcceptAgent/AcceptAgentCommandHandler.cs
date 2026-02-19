using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.UseCases.Commands.AcceptAgent;

/// <summary>
/// هندلر سرویس تائید نماینده
/// </summary>
internal class AcceptAgentCommandHandler(
    ILcpExternalService lcpExternalService,
    IMapper mapper)
    : ICommandHandler<
        AcceptAgentCommand,
        AcceptAgentDto>
{
    public async Task<AcceptAgentDto> Handle(
        AcceptAgentCommand command,
        CancellationToken cancellationToken)
    {
        return await lcpExternalService.AcceptAgent(
            mapper.Map<AcceptAgentDm>(command),
            cancellationToken);
    }
}