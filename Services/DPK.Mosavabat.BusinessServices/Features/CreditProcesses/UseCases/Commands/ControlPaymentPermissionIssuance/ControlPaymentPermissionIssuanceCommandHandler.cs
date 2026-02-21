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

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.ControlPaymentPermissionIssuance;

/// <summary>
/// هندلر ایجاد شناسه پرداخت
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
        string? ControlIssuanceResult = null);

    public async Task<Result<InsertPaymentPermissionDto>> Handle(
        InsertPaymentPermissionCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => CheckPaymentPermissionIssuanceAsync(ctx, cancellationToken))
            .Bind(ctx => CheckPayamInquiryAsync(ctx, cancellationToken))
            .Bind(ctx => InsertPaymentPermissionInLmsAsync(ctx, cancellationToken));
    }

    private async Task<Result<PipelineContext>> CheckPaymentPermissionIssuanceAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var command = context.Command;

        var response = await creditProcessSqlService.ControlPaymentPermissionIssuance(
            new ControlPaymentPermissionIssuanceDm(
                command.MosavabeNo,
                command.ContractAmount,
                command.PrepaidAmount,
                command.SubContractType,
                command.CustomerNo),
            cancellationToken);

        var validationResult = ValidateBusinessRule(response.Result);
        return validationResult.IsFailure
            ? Result.Failure<PipelineContext>(validationResult.Error)
            : Result.Success(context with { ControlIssuanceResult = response.Result });
    }

    private async Task<Result<PipelineContext>> CheckPayamInquiryAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var response = await payamExternalService.CheckInquiry(
            new CheckInquiryDm(context.ControlIssuanceResult),
            cancellationToken);

        var validationResult = ValidateBusinessRule(response.BouncedCheckCount);
        return validationResult.IsFailure
            ? Result.Failure<PipelineContext>(validationResult.Error)
            : Result.Success(context);
    }

    private async Task<Result<InsertPaymentPermissionDto>> InsertPaymentPermissionInLmsAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var dm = mapper.Map<InsertPaymentPermissionDm>(context.Command);

        var response = await lmsExternalService.InsertPaymentPermission(
            dm,
            cancellationToken);

        return Result.Success(response);
    }

    private Result ValidateBusinessRule(string? result)
    {
        if (string.IsNullOrEmpty(result) ||
            result.Length > 5 ||
            result.Equals("true", StringComparison.OrdinalIgnoreCase) ||
            result.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Success();
        }

        if (!int.TryParse(result, out var code))
        {
            return Result.Success();
        }

        return messageProvider.TryGetMessage(code, out var message, out var errorEnum)
            ? Result.Failure(new Error($"BusinessRule.{errorEnum}", message))
            : Result.Success();
    }
}
