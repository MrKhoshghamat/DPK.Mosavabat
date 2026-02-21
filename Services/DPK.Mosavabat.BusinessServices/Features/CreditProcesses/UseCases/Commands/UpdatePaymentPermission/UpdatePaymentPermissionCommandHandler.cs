using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.UpdatePaymentPermission;

/// <summary>
/// هندلر سرویس به‌روزرسانی مجوز پرداخت
/// </summary>
internal class UpdatePaymentPermissionCommandHandler(
    ILmsExternalService lmsExternalService,
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : ICommandHandler<
        UpdatePaymentPermissionCommand,
        Result<UpdatePaymentPermissionDto>>
{
    private record PipelineContext(
        UpdatePaymentPermissionCommand Command,
        UpdatePaymentPermissionDto LmsResponse);

    public async Task<Result<UpdatePaymentPermissionDto>> Handle(
        UpdatePaymentPermissionCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(command)
            .Bind(cmd => UpdateLmsPermissionAsync(cmd, cancellationToken))
            .Bind(ctx => UpdateLocalDatabaseIfCanceledAsync(ctx, cancellationToken))
            .Map(ctx => ctx.LmsResponse);
    }

    private async Task<Result<PipelineContext>> UpdateLmsPermissionAsync(
        UpdatePaymentPermissionCommand command,
        CancellationToken cancellationToken)
    {
        var mappedDm = mapper.Map<UpdatePaymentPermissionDm>(command);

        var response = await lmsExternalService.UpdatePaymentPermission(
            mappedDm,
            cancellationToken);

        return Result.Success(new PipelineContext(command, response));
    }

    private async Task<Result<PipelineContext>> UpdateLocalDatabaseIfCanceledAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        if (!context.Command.IsCanceled)
        {
            return Result.Success(context);
        }

        await creditProcessSqlService.UpdateIsDeletedCuContract(
            new UpdateIsDeletedCuContractDm(
                context.Command.Id,
                context.Command.IsCanceled),
            cancellationToken);

        return Result.Success(context);
    }
}
