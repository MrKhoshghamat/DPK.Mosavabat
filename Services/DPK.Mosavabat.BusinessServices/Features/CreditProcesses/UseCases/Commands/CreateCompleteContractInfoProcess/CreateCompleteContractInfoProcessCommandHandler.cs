using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CreateCompleteContractInfoProcess;

/// <summary>
/// هندلر سرویس ساخت فرایند تکمیل اطلاعات قرارداد 
/// </summary>
internal class CreateCompleteContractInfoProcessCommandHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IWorkflowEngineService workflowEngineService)
    : ICommandHandler<
        CreateCompleteContractInfoProcessCommand,
        Result<CreateCompleteContractInfoProcessDto>>
{
    private const string ProcessName = "MainProcess";
    private const string EntityName = "CU_Contract";

    public record PipelineContext(
        CreateCompleteContractInfoProcessCommand Command,
        string BranchCode = "",
        string Username = "",
        string AccNo = "",
        DateTime DateG = default,
        string C16 = "",
        long CaseId = 0,
        string RadNumber = "");

    public async Task<Result<CreateCompleteContractInfoProcessDto>> Handle(
        CreateCompleteContractInfoProcessCommand command,
        CancellationToken cancellationToken)
    {
        var initialContext = new PipelineContext(command);

        return await Result.Success(initialContext)
            .Bind(context => FetchBranchCodeAsync(context, cancellationToken))
            .Bind(context => FetchResponsibleUserAsync(context, cancellationToken))
            .Bind(context => FetchAdditionalContractDataParallelAsync(context, cancellationToken))
            .Bind(CreateBizagiCaseAsync)
            .Bind(context => UpdateLocalDatabaseSafeAsync(context, cancellationToken))
            .Map(CreateFinalResponse);
    }

    private async Task<Result<PipelineContext>> FetchBranchCodeAsync(
        PipelineContext context, CancellationToken cancellationToken)
    {
        var branchDto = await creditProcessSqlService.GetBranchCodeByC13(
            new GetBranchCodeByC13Dm(context.Command.C13), cancellationToken);

        return !int.TryParse(branchDto.BranchCode, out _)
            ? Result.Failure<PipelineContext>(
                new Error(
                    "InvalidBranchCode",
                    "کد شعبه نامعتبر است."))
            : Result.Success(context with { BranchCode = branchDto.BranchCode });
    }

    private async Task<Result<PipelineContext>> FetchResponsibleUserAsync(
        PipelineContext context, CancellationToken cancellationToken)
    {
        var userDto = await creditProcessSqlService.FindBranchManagerOrSupervisor(
            new FindBranchManagerOrSupervisorDm(int.Parse(context.BranchCode), MosavabatClassName.CompleteContractInfo),
            cancellationToken);

        return !string.IsNullOrEmpty(userDto.Username)
            ? Result.Success(context with { Username = userDto.Username })
            : Result.Failure<PipelineContext>(
                new Error(
                    "UserNotFound",
                    "کاربر مسئول یافت نشد."));
    }

    private async Task<Result<PipelineContext>> FetchAdditionalContractDataParallelAsync(
        PipelineContext context, CancellationToken cancellationToken)
    {
        var accNoTask = creditProcessSqlService.GetAccNoByC13(new GetAccNoByC13Dm(context.Command.C13), cancellationToken);
        var dateGTask = creditProcessSqlService.GetDateGByC13(new GetDateGByC13Dm(context.Command.C13), cancellationToken);
        var c16Task = creditProcessSqlService.GetC16ByC13(new GetC16ByC13Dm(context.Command.C13), cancellationToken);

        await Task.WhenAll(
            accNoTask,
            dateGTask,
            c16Task);

        return Result.Success(context with
        {
            AccNo = accNoTask.Result.AccNo,
            DateG = dateGTask.Result.DateG,
            C16 = c16Task.Result.C16
        });
    }

    private async Task<Result<PipelineContext>> CreateBizagiCaseAsync(PipelineContext context)
    {
        var bizagiPayload = new CompleteContractInfoBizagiDm
        {
            MabAsl = context.Command.MabAsl,
            CuApprovalSubjectId = context.Command.CuApprovalSubjectId,
            C10 = context.Command.C10,
            Shob = context.BranchCode,
            C13 = context.Command.C13,
            C16 = context.C16,
            DateG = context.DateG
        };

        var caseResult = await workflowEngineService.CreateCaseAsync(
            model: bizagiPayload,
            processName: ProcessName,
            entityName: EntityName,
            userName: context.Username
        );

        if (caseResult.IsFailure)
        {
            return Result.Failure<PipelineContext>(caseResult.Error);
        }

        if (caseResult.Value.CaseId == 0 || caseResult.Value.RadNumber == "0")
        {
            return Result.Failure<PipelineContext>(
                new Error("BizagiError", "خطا در ایجاد فرایند در سامانه مصوبات. خروجی نامعتبر است."));
        }

        return Result.Success(context with
        {
            CaseId = caseResult.Value.CaseId,
            RadNumber = caseResult.Value.RadNumber
        });
    }

    private async Task<Result<PipelineContext>> UpdateLocalDatabaseSafeAsync(
        PipelineContext context, CancellationToken cancellationToken)
    {
        try
        {
            var customerEkDto = await creditProcessSqlService.GetCustomerEk(
                new GetCustomerEkDm(context.AccNo), cancellationToken);

            await creditProcessSqlService.UpdateCuCustomerInCuContract(
                new UpdateCuCustomerInCuContractDm(
                    customerEkDto.IdCuCustomerEk,
                    context.Command.C13,
                    context.C16),
                cancellationToken);
        }
        catch (Exception)
        {
            // ignored
        }

        return Result.Success(context);
    }

    private static CreateCompleteContractInfoProcessDto CreateFinalResponse(
        PipelineContext context)
    {
        return new CreateCompleteContractInfoProcessDto(
            context.CaseId,
            context.RadNumber,
            DateTime.Now,
            "فرایند با موفقیت ایجاد شد."
        );
    }
}
