using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.UseCases.Commands.CreateContract;

/// <summary>
/// هندلر سرویس ساخت قرارداد
/// </summary>
internal class CreateContractCommandHandler(
    ILcpExternalService lcpExternalService,
    IMapper mapper)
    : ICommandHandler<
        CreateContractCommand,
        CreateLcpContractDto>
{
    public async Task<CreateLcpContractDto> Handle(
        CreateContractCommand command,
        CancellationToken cancellationToken)
    {
        return await lcpExternalService.CreateContract(
            mapper.Map<CreateLcpContractDm>(command),
            cancellationToken);
    }
}