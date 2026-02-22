using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.Agents;
using DPK.Mosavabat.Models.DataModels.Agents;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.Users;
using DPK.Mosavabat.Models.EntitiesDto.Agents;
using DPK.Mosavabat.SqlBusinessServices.Agents.Contract;
using DPK.Mosavabat.SqlBusinessServices.Users.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.Agents.UseCases.Commands.CreateAgent;

/// <summary>
/// هندلر سرویس ایجاد نماینده
/// </summary>
internal class CreateAgentCommandHandler(
    IUserSqlService userSqlService,
    IAgentSqlService agentSqlService,
    IWorkflowEngineService workflowEngineService,
    IMapper mapper)
    : ICommandHandler<
        CreateAgentCommand,
        Result<CreateAgentDto>>
{
    private record PipelineContext(
        CreateAgentCommand Command,
        string? Username = null,
        int CaseId = 0,
        int RadNumber = 0);

    public async Task<Result<CreateAgentDto>> Handle(
        CreateAgentCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => FetchUsernameAsync(ctx, cancellationToken))
            .Bind(CreateBizagiCaseAsync)
            .Bind(ctx => UpdateAgentRegistryAsync(ctx, cancellationToken))
            .Map(CreateFinalResponse);
    }

    private async Task<Result<PipelineContext>> FetchUsernameAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(context.Command.BranchCode, out var branchCodeId))
        {
            return Result.Failure<PipelineContext>(
                new Error("Validation.Error", "کد شعبه نامعتبر است. لطفاً فقط عدد وارد کنید."));
        }

        var usernameResult = await userSqlService.GetUsernameByBranchCode(
            new GetUsernameByBranchCodeDm(
                branchCodeId,
                MosavabatClassName.AgentRegistry),
            cancellationToken);

        return usernameResult.IsFailure
            ? Result.Failure<PipelineContext>(usernameResult.Error)
            : Result.Success(context with { Username = usernameResult.Value.Username });
    }

    private async Task<Result<PipelineContext>> CreateBizagiCaseAsync(
        PipelineContext context)
    {
        var bizagiDm = mapper.Map<CreateAgentBizagiDm>(context.Command);

        var caseResult = await workflowEngineService.CreateCaseAsync(
            bizagiDm,
            "MainProcess",
            "AgentRegistry",
            context.Username!);

        if (caseResult.IsFailure)
        {
            return Result.Failure<PipelineContext>(caseResult.Error);
        }

        if (!int.TryParse(caseResult.Value.CaseId.ToString(), out var caseId) ||
            !int.TryParse(caseResult.Value.RadNumber, out var radNumber))
        {
            return Result.Failure<PipelineContext>(
                new Error("Bizagi.ParseError", "شناسه کیس یا شماره راد دریافتی از بیزاجی معتبر نیست."));
        }

        return Result.Success(context with { CaseId = caseId, RadNumber = radNumber });
    }

    private async Task<Result<PipelineContext>> UpdateAgentRegistryAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var agentDm = new AgentDm(
            context.Command.BranchCode,
            context.Command.NationalCode,
            context.Command.LastName,
            context.Command.Mobile,
            context.Command.Name,
            context.Command.LegalNationalId,
            context.Command.LegalName);

        var updateResult = await agentSqlService.UpdateCaseIdAndRadNumberInAgentRegistry(
            new UpdateCaseIdAndRadNumberInAgentRegistryDm(
                agentDm,
                context.CaseId,
                context.RadNumber),
            cancellationToken);

        return updateResult.IsFailure
            ? Result.Failure<PipelineContext>(updateResult.Error)
            : Result.Success(context);
    }

    private static CreateAgentDto CreateFinalResponse(PipelineContext context)
    {
        return new CreateAgentDto(
            context.CaseId,
            context.RadNumber.ToString(),
            DateTime.Now,
            "فرایند با موفقیت انجام شد");
    }
}
