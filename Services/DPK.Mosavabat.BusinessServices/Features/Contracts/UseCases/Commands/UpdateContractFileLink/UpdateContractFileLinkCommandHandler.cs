using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Commands.Contracts;
using DPK.Mosavabat.Models.DataModels.Contracts;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;
using DPK.Mosavabat.SqlBusinessServices.Contracts.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.Contracts.UseCases.Commands.UpdateContractFileLink;

/// <summary>
/// هندلر سرویس به‌روزرسانی لینک فایل‌های قرارداد
/// </summary>
internal class UpdateContractFileLinkCommandHandler(
    IContractSqlService contractSqlService,
    IMapper mapper)
    : ICommandHandler<
        UpdateContractFileLinkCommand,
        Result<UpdateContractFileLinkDto>>
{
    public async Task<Result<UpdateContractFileLinkDto>> Handle(
        UpdateContractFileLinkCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(command)
            .Map(mapper.Map<UpdateContractFileLinkDm>)
            .Bind(dm => contractSqlService
                .UpdateContractFileLink(
                    dm,
                    cancellationToken));
    }
}