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

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.UseCases.Commands.AcceptRequestFacility;

/// <summary>
/// هندلر سرویس تائید درخواست تسهیلات
/// </summary>
internal class AcceptRequestFacilityCommandHandler(
    ILcpExternalService lcpExternalService,
    IMapper mapper)
    : ICommandHandler<
        AcceptRequestFacilityCommand,
        Result<UpdateLcpRequestFacilityStatusDto>>
{
    public async Task<Result<UpdateLcpRequestFacilityStatusDto>> Handle(
        AcceptRequestFacilityCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(command)
            .Map(mapper.Map<UpdateLcpRequestFacilityStatusDm>)
            .Bind(dm => lcpExternalService.UpdateLcpRequestFacilityStatus(
                dm,
                cancellationToken));
    }
}