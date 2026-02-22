using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.Agents;
using DPK.Mosavabat.Models.DataModels.Agents;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.EntitiesDto.Agents;
using DPK.Mosavabat.SqlBusinessServices.Agents.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.Agents.UseCases.Commands.RemoveAgentFromCartable;

/// <summary>
/// هندلر سرویس حذف نماینده از کارتابل
/// </summary>
internal class RemoveAgentFromCartableCommandHandler(
    IAgentSqlService agentSqlService,
    IWorkflowEngineService workflowEngineService)
    : ICommandHandler<
        RemoveAgentFromCartableCommand,
        Result<RemoveAgentFromCartableDto>>
{
    public record PipelineContext(
        RemoveAgentFromCartableCommand Command,
        string Username = "",
        string RadNumber = "",
        int UserId = 0);

    public async Task<Result<RemoveAgentFromCartableDto>> Handle(
        RemoveAgentFromCartableCommand command,
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
        var usernameTask = agentSqlService.GetUserNameByCaseId(
            new GetUserNameByCaseIdDm(context.Command.CaseId),
            cancellationToken);

        var radNumberTask = agentSqlService.GetRadNumberByCaseId(
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
        var userIdResult = await agentSqlService.GetUserIdByUsername(
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
        var localDbResult = await agentSqlService.AbortAgentCase(
            new AbortAgentCaseDm(context.Command.CaseId),
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

    private static RemoveAgentFromCartableDto CreateFinalResponse(
        PipelineContext context)
    {
        return new RemoveAgentFromCartableDto(
            IsRemoved: true,
            Message: "درخواست با موفقیت از کارتابل حذف و لغو گردید.",
            ProcessDate: DateTime.Now
        );
    }
}
