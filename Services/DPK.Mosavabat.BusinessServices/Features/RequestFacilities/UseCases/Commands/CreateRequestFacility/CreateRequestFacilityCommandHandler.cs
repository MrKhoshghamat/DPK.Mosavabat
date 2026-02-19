using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.CreateRequestFacility;

/// <summary>
/// هندلر سرویس ایجاد درخواست تسهیلات
/// </summary>
internal sealed class CreateRequestFacilityCommandHandler
    : ICommandHandler<
        CreateRequestFacilityCommand,
        Result<CreateRequestFacilityDto>>
{

    public async Task<Result<CreateRequestFacilityDto>> Handle(
        CreateRequestFacilityCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
