using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.RemoveRequestFromCartable;

/// <summary>
/// هندلر سرویس حذف درخواست از کارتابل
/// </summary>
internal sealed class RemoveRequestFromCartableCommandHandler(
    IRequestFacilitySqlService requestFacilitySqlService,
    IWorkflowEngineService workflowEngineService)
    : ICommandHandler<RemoveRequestFromCartableCommand, Result<RemoveRequestFromCartableDto>>
{
    public record PipelineContext(
        RemoveRequestFromCartableCommand Command,
        string Username = "",
        string RadNumber = "",
        int UserId = 0);

    public async Task<Result<RemoveRequestFromCartableDto>> Handle(
        RemoveRequestFromCartableCommand command,
        CancellationToken cancellationToken)
    {
        var initialContext = new PipelineContext(command);

        return await Result.Success(initialContext)
            .Bind(context => FetchInitialCaseDetailsAsync(context, cancellationToken))
            .Bind(context => FetchUserIdAsync(context, cancellationToken))
            .Bind(AbortBizagiCaseAsync)
            .Bind(context => AbortLocalDatabaseCaseAsync(context, cancellationToken))
            .Map(CreateFinalResponse);
    }

    private async Task<Result<PipelineContext>> FetchInitialCaseDetailsAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var usernameTask = requestFacilitySqlService.GetUserNameByCaseId(
            new GetUserNameByCaseIdDm(context.Command.CaseId),
            cancellationToken);

        var radNumberTask = requestFacilitySqlService.GetRadNumberByCaseId(
            new GetRadNumberByCaseIdDm(context.Command.CaseId),
            cancellationToken);

        await Task.WhenAll(usernameTask, radNumberTask);

        if (usernameTask.Result.IsFailure)
            return Result.Failure<PipelineContext>(usernameTask.Result.Error);

        if (radNumberTask.Result.IsFailure)
            return Result.Failure<PipelineContext>(radNumberTask.Result.Error);

        return Result.Success(context with
        {
            Username = usernameTask.Result.Value.Username,
            RadNumber = radNumberTask.Result.Value.RadNumber.ToString()
        });
    }

    private async Task<Result<PipelineContext>> FetchUserIdAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var userIdResult = await requestFacilitySqlService.GetUserIdByUsername(
            new GetUserIdByUsernameDm(context.Username),
            cancellationToken);

        return userIdResult.IsFailure
            ? Result.Failure<PipelineContext>(userIdResult.Error)
            : Result.Success(context with { UserId = userIdResult.Value.UserId });
    }

    private async Task<Result<PipelineContext>> AbortBizagiCaseAsync(
        PipelineContext context)
    {
        var abortCaseBizagiDm = new AbortCaseBizagiDm
        {
            UserName = context.Username,
            UserId = context.UserId.ToString(),
            Cases = new CasesBizagiDm
            {
                Entities =
                [
                    new CaseBizagiDm
                    {
                        RadNumber = context.RadNumber,
                        AbortReason = "RemoveRequestFromCartable"
                    }
                ]
            }
        };

        var bizagiResult = await workflowEngineService.AbortCaseAsync(abortCaseBizagiDm);

        return bizagiResult.IsFailure
            ? Result.Failure<PipelineContext>(bizagiResult.Error)
            : Result.Success(context);
    }

    private async Task<Result<PipelineContext>> AbortLocalDatabaseCaseAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var localDbResult = await requestFacilitySqlService.AbortRequestFacilityCase(
            new AbortRequestFacilityCaseDm(context.Command.CaseId),
            cancellationToken);

        if (localDbResult.IsFailure)
            return Result.Failure<PipelineContext>(localDbResult.Error);

        return !localDbResult.Value.IsAborted
            ? Result.Failure<PipelineContext>(
                new Error(
                    "DatabaseUpdateFailed",
                    "عملیات لغو در پایگاه داده محلی ناموفق بود."))
            : Result.Success(context);
    }

    private static RemoveRequestFromCartableDto CreateFinalResponse(
        PipelineContext context)
    {
        return new RemoveRequestFromCartableDto(
            IsRemoved: true,
            Message: "درخواست با موفقیت از کارتابل حذف و لغو گردید.",
            ProcessDate: DateTime.Now
        );
    }
}
