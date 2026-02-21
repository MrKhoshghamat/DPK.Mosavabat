using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
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
        Result<CreateLcpContractDto>>
{
    public async Task<Result<CreateLcpContractDto>> Handle(
        CreateContractCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(command)
            .Map(mapper.Map<CreateLcpContractDm>)
            .Bind(dm => lcpExternalService.CreateContract(
                dm,
                cancellationToken));
    }
}