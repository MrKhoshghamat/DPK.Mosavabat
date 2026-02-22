using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Rules;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.Models.Commands.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.Users;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;
using DPK.Mosavabat.SqlBusinessServices.Users.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.UseCases.Commands.CreateApprovalRequest;

/// <summary>
/// هندلر سرویس ایجاد درخواست صدور مصوبه
/// </summary>
internal sealed class CreateApprovalRequestCommandHandler(
    IApprovalRequestSqlService approvalRequestSqlService,
    IUserSqlService userSqlService,
    IWorkflowEngineService workflowEngineService,
    IRequestFacilitySqlService requestFacilitySqlService,
    ILcpExternalService lcpExternalService)
    : ICommandHandler<
        CreateApprovalRequestCommand,
        Result<CreateApprovalRequestDto>>
{
    private const string ProcessName = "MainProcess";
    private const string EntityName = "approvalsRequest";

    private record PipelineContext(
        CreateApprovalRequestCommand Command,
        string? Username = null,
        int CaseId = 0,
        int RadNumber = 0);

    public async Task<Result<CreateApprovalRequestDto>> Handle(
        CreateApprovalRequestCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => FetchBranchCodeAndUsernameAsync(ctx, cancellationToken))
            .Bind(CreateBizagiCaseSafeAsync)
            .Bind(ctx => ExecuteParallelDatabaseUpdatesAsync(ctx, cancellationToken))
            .Map(CreateFinalResponse);
    }

    private async Task<Result<PipelineContext>> FetchBranchCodeAndUsernameAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var branchCodeResult = await approvalRequestSqlService.GetBranchCodeByLcpRequestFacilityId(
            new GetBranchCodeByLcpRequestFacilityIdDm(context.Command.LcpRequestFacilityId),
            cancellationToken);

        if (branchCodeResult.IsFailure) return Result.Failure<PipelineContext>(branchCodeResult.Error);

        var validBranchCodeResult = ApprovalRequestRules.ValidBranchCode(branchCodeResult.Value.BranchCode);
        if (validBranchCodeResult.IsFailure) return Result.Failure<PipelineContext>(validBranchCodeResult.Error);

        var branchCode = validBranchCodeResult.Value;

        var usernameResult = await userSqlService.GetUsernameByBranchCode(
            new GetUsernameByBranchCodeDm(branchCode, MosavabatClassName.ApprovalRequest2),
            cancellationToken);

        if (usernameResult.IsFailure) return Result.Failure<PipelineContext>(usernameResult.Error);

        var validUsernameResult = ApprovalRequestRules.UsernameExists(usernameResult.Value.Username);
        if (validUsernameResult.IsFailure) return Result.Failure<PipelineContext>(validUsernameResult.Error);

        return Result.Success(context with
        {
            Username = validUsernameResult.Value
        });
    }

    private async Task<Result<PipelineContext>> CreateBizagiCaseSafeAsync(
        PipelineContext context)
    {
        var caseResult = await workflowEngineService.CreateCaseAsync(
            new CreateApprovalRequestBizagiDm
            {
                ApprovalRequestGroup = "1",
                RequestTypeDetail = "1"
            },
            processName: ProcessName,
            entityName: EntityName,
            userName: context.Username!);

        if (caseResult.IsFailure) return Result.Failure<PipelineContext>(caseResult.Error);

        var validCaseResult = ApprovalRequestRules.ValidBizagiCase(caseResult.Value);
        if (validCaseResult.IsFailure) return Result.Failure<PipelineContext>(validCaseResult.Error);

        if (!int.TryParse(caseResult.Value.CaseId.ToString(), out var caseId) ||
            !int.TryParse(caseResult.Value.RadNumber, out var radNumber))
        {
            return Result.Failure<PipelineContext>(
                new Error("Bizagi.InvalidData", "شناسه کیس یا شماره راد دریافتی از بیزاجی فاقد فرمت عددی معتبر است."));
        }

        return Result.Success(context with { CaseId = caseId, RadNumber = radNumber });
    }

    private async Task<Result<PipelineContext>> ExecuteParallelDatabaseUpdatesAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var cmd = context.Command;

        var lcpUpdateTask = lcpExternalService.UpdateLcpRequestFacilityRadNumber(
            new UpdateLcpRequestFacilityRadNumberDm(
                cmd.LcpRequestFacilityIdCase,
                context.CaseId,
                context.CaseId),
            cancellationToken);

        var reqFacilityUpdateTask = requestFacilitySqlService.UpdateLcpRequestFacility(
            new UpdateLcpRequestFacilityDm(
                cmd.LcpRequestFacilityIdCase,
                cmd.LcpRequestFacilityRadNumber,
                context.CaseId,
                context.RadNumber,
                cmd.LcpRequestFacilityId,
                cmd.GuidLcpRequestFacility),
            cancellationToken);

        var mappingTask = requestFacilitySqlService.MapLcpRequestFacilityToApprovalRequest(
            new MapLcpRequestFacilityToApprovalRequestDm(
                cmd.LcpRequestFacilityId,
                context.CaseId,
                cmd.LcpRequestFacilityIdCase),
            cancellationToken);

        await Task.WhenAll(
            lcpUpdateTask,
            reqFacilityUpdateTask,
            mappingTask);

        var results = new Result[]
        {
            lcpUpdateTask.Result,
            reqFacilityUpdateTask.Result,
            mappingTask.Result
        };
        var failedResult = results.FirstOrDefault(r => r.IsFailure);

        return failedResult is not null
            ? Result.Failure<PipelineContext>(failedResult.Error)
            : Result.Success(context);
    }

    private static CreateApprovalRequestDto CreateFinalResponse(PipelineContext context)
    {
        return new CreateApprovalRequestDto(
            context.CaseId,
            context.RadNumber.ToString(),
            DateTime.Now,
            "فرایند با موفقیت ایجاد شد");
    }
}
