using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.UpdateRequestFacilityRadNumber;

/// <summary>
/// هندلر سرویس ویرایش شماره پیگیری درخواست تسهیلات
/// </summary>
internal class UpdateRequestFacilityRadNumberCommandHandler(
    IRequestFacilitySqlService requestFacilitySqlService,
    IMapper mapper)
    : ICommandHandler<
        UpdateRequestFacilityRadNumberCommand,
        Result<UpdateRequestFacilityRadNumberDto>>
{
    public Task<Result<UpdateRequestFacilityRadNumberDto>> Handle(
        UpdateRequestFacilityRadNumberCommand command,
        CancellationToken cancellationToken)
    {
        return requestFacilitySqlService.UpdateRequestFacilityRadNumber(
            mapper.Map<UpdateRequestFacilityRadNumberDm>(command),
            cancellationToken);
    }
}
