using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
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
        UpdateLcpRequestFacilityStatusDto>
{
    public async Task<UpdateLcpRequestFacilityStatusDto> Handle(
        AcceptRequestFacilityCommand command,
        CancellationToken cancellationToken)
    {
        return await lcpExternalService.UpdateLcpRequestFacilityStatus(
            mapper.Map<UpdateLcpRequestFacilityStatusDm>(command),
            cancellationToken);
    }
}