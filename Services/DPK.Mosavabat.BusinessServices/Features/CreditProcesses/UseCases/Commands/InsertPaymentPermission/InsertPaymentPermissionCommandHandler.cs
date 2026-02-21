using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Abstractions.Errors;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.ExternalServices.Payam.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.Payam;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.InsertPaymentPermission;

/// <summary>
/// هندلر سرویس ایجاد شناسه پرداخت
/// </summary>
internal class InsertPaymentPermissionCommandHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IPayamExternalService payamExternalService,
    ICreditProcessMessageProvider messageProvider,
    ILmsExternalService lmsExternalService,
    IMapper mapper)
    : ICommandHandler<
        InsertPaymentPermissionCommand,
        Result<InsertPaymentPermissionDto>>
{
    private record PipelineContext(
        InsertPaymentPermissionCommand Command,
        string? SqlControlResult = null,
        string? BouncedCheckCount = null);

    public async Task<Result<InsertPaymentPermissionDto>> Handle(
        InsertPaymentPermissionCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => ControlPaymentPermissionAsync(ctx, cancellationToken))
            .Bind(ctx => CheckPayamInquiryAsync(ctx, cancellationToken))
            .Bind(ctx => InsertPermissionInLmsAsync(ctx, cancellationToken));
    }

    private async Task<Result<PipelineContext>> ControlPaymentPermissionAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var response = await creditProcessSqlService
            .ControlPaymentPermissionIssuance(
                new ControlPaymentPermissionIssuanceDm(
                    context.Command.MosavabeNo,
                    context.Command.ContractAmount,
                    context.Command.PrepaidAmount,
                    context.Command.SubContractType,
                    context.Command.CustomerNo),
                cancellationToken);

        var validationResult = EvaluateBusinessRule(response.Result);
        return validationResult.IsFailure
            ? Result.Failure<PipelineContext>(validationResult.Error)
            : Result.Success(context with { SqlControlResult = response.Result });
    }

    private async Task<Result<PipelineContext>> CheckPayamInquiryAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var response = await payamExternalService.CheckInquiry(
            new CheckInquiryDm(context.SqlControlResult!),
            cancellationToken);

        var validationResult = EvaluateBusinessRule(response.BouncedCheckCount);
        return validationResult.IsFailure
            ? Result.Failure<PipelineContext>(validationResult.Error)
            : Result.Success(context with { BouncedCheckCount = response.BouncedCheckCount });
    }

    private async Task<Result<InsertPaymentPermissionDto>> InsertPermissionInLmsAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var mappedDm = mapper.Map<InsertPaymentPermissionDm>(context.Command);

        var response = await lmsExternalService.InsertPaymentPermission(
            mappedDm,
            cancellationToken);

        return Result.Success(response);
    }

    private Result EvaluateBusinessRule(string? valueToValidate)
    {
        if (string.IsNullOrEmpty(valueToValidate) ||
            valueToValidate.Length > 5 ||
            valueToValidate.Equals("true", StringComparison.OrdinalIgnoreCase) ||
            valueToValidate.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Success();
        }

        if (!int.TryParse(valueToValidate, out var code))
        {
            return Result.Success();
        }

        if (!messageProvider.TryGetMessage(code, out var message, out var errorEnum)) return Result.Success();
        var errorCode = errorEnum?.ToString() ?? "Business.RuleViolation";
        return Result.Failure(new Error(errorCode, message));

    }
}
