using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Helpers;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.UpdateRequestFacility;

/// <summary>
/// هندلر سرویس ویرایش درخواست تسهیلات
/// </summary>
internal sealed class UpdateRequestFacilityCommandHandler(
    IRequestFacilitySqlService requestFacilitySqlService,
    IWorkflowEngineService workflowEngineService,
    IMapper mapper)
    : ICommandHandler<UpdateRequestFacilityCommand, Result<UpdateRequestFacilityDto>>
{
    // ۱. اضافه شدن فیلدهای نهایی برای نگهداری مقادیر برگشتی از بیزاجی
    public record PipelineContext(
        UpdateRequestFacilityCommand Command,
        UpdateRequestFacilityBizagiDm BizagiDm,
        int ApprovalRequestId = 0,
        long FinalCaseId = 0,
        string FinalRadNumber = "");

    public async Task<Result<UpdateRequestFacilityDto>> Handle(
        UpdateRequestFacilityCommand command,
        CancellationToken cancellationToken)
    {
        var initialBizagiDm = mapper.Map<UpdateRequestFacilityBizagiDm>(command);
        var initialContext = new PipelineContext(command, initialBizagiDm);

        return await Result.Success(initialContext)
            .Bind(context => FetchApprovalRequestIdAsync(context, cancellationToken))
            .Bind(ProcessHistoryLogs)
            .Bind(context => UpdateLocalDatabaseStatusAsync(context, cancellationToken))
            .Bind(UpdateBizagiActivityAsync)
            .Map(CreateFinalResponse);
    }

    private async Task<Result<PipelineContext>> FetchApprovalRequestIdAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(context.Command.CaseId, out var caseId))
            return Result.Failure<PipelineContext>(
                new Error("InvalidCaseId",
                    "فرمت شناسه پرونده نامعتبر است."));

        var result = await requestFacilitySqlService.GetApprovalRequestId(
            new GetApprovalRequestIdDm(caseId),
            cancellationToken);

        return result.IsFailure
            ? Result.Failure<PipelineContext>(result.Error)
            : Result.Success(context with { ApprovalRequestId = result.Value.ApprovalRequestId });
    }

    private static Result<PipelineContext> ProcessHistoryLogs(PipelineContext context)
    {
        if (string.IsNullOrWhiteSpace(context.Command.SummarizeDifference))
            return Result.Success(context);

        try
        {
            var raw = context.Command.SummarizeDifference;
            var diff = JsonHelper.ParseJsonString(raw);
            var summarizeDifference = JsonHelper.ExtractFlatFieldChanges(diff);

            context.BizagiDm.LcpHistoryLogs.LcpHistoryLogs = summarizeDifference
                .Select(sd =>
                    new LcpHistoryLogBizagiDm
                    {
                        ApprovalRequestId = context.ApprovalRequestId.ToString(),
                        NewValue = sd.NewValue,
                        OldValue = sd.OldValue,
                        Title = sd.Path
                    }).ToList();

            return Result.Success(context);
        }
        catch (Exception)
        {
            return Result.Failure<PipelineContext>(
                new Error("JsonParseError", "خطا در پردازش اطلاعات لاگ تغییرات."));
        }
    }

    private async Task<Result<PipelineContext>> UpdateLocalDatabaseStatusAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var result = await requestFacilitySqlService.UpdateRequestFacilityStatus(
            new UpdateRequestFacilityStatusDm(int.Parse(context.Command.CaseId)),
            cancellationToken);

        return result.IsFailure
            ? Result.Failure<PipelineContext>(result.Error)
            : Result.Success(context);
    }

    private async Task<Result<PipelineContext>> UpdateBizagiActivityAsync(
        PipelineContext context)
    {
        context.BizagiDm.LcpApprovalStatus = "5";

        var caseResult = await workflowEngineService.SaveActivityAsync(context.BizagiDm);

        if (caseResult.IsFailure)
            return Result.Failure<PipelineContext>(caseResult.Error);

        return Result.Success(context with
        {
            FinalCaseId = caseResult.Value.CaseId,
            FinalRadNumber = caseResult.Value.RadNumber
        });
    }

    private static UpdateRequestFacilityDto CreateFinalResponse(
        PipelineContext context)
    {
        return new UpdateRequestFacilityDto(
            CaseId: context.FinalCaseId,
            RadNumber: context.FinalRadNumber,
            ProcessDate: DateTime.Now,
            Message: "ویرایش درخواست تسهیلات با موفقیت انجام شد."
        );
    }
}
