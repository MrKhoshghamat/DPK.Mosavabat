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

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.UseCases.Commands.UpdateRequestFacilityRadNumber;

/// <summary>
/// هندلر سرویس بروزرسانی شماره پیگیری ثبت درخواست تسهیلات
/// </summary>
internal class UpdateRequestFacilityRadNumberCommandHandler(
    ILcpExternalService lcpExternalService,
    IMapper mapper)
    : ICommandHandler<
        UpdateRequestFacilityRadNumberCommand,
        Result<UpdateLcpRequestFacilityRadNumberDto>>
{
    public async Task<Result<UpdateLcpRequestFacilityRadNumberDto>> Handle(
        UpdateRequestFacilityRadNumberCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(command)
            .Map(mapper.Map<UpdateLcpRequestFacilityRadNumberDm>)
            .Bind(dm => lcpExternalService.UpdateLcpRequestFacilityRadNumber(
                dm,
                cancellationToken));
    }
}