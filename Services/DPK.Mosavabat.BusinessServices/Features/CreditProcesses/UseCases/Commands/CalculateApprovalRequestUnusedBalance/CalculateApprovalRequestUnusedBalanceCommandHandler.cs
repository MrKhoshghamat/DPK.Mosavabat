using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Services.LoanBills.LoanBillProcessing.Contract;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.GeneralServices.CreditProcesses.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CalculateApprovalRequestUnusedBalance;

/// <summary>
/// هندلر محاسبه مانده آزاد مصوبه
/// </summary>
internal class CalculateApprovalRequestUnusedBalanceCommandHandler(
    ICreditProcessSqlService creditProcessSqlService,
    ILoanBillProcessingService loanBillProcessingService,
    ICreditProcessGeneralService creditProcessGeneralService,
    IMapper mapper)
    : ICommandHandler<
        CalculateApprovalRequestUnusedBalanceCommand,
        Result<CalculateApprovalRequestUnusedBalanceDto>>
{
    private record PipelineContext(
        CalculateApprovalRequestUnusedBalanceCommand Command,
        string? CustomerNo = null,
        long DailySettledAmount = 0,
        string? DailyReceivedAmount = null,
        string? SqlBaseAmount = null,
        int RemainingContractsCount = 0);

    public async Task<Result<CalculateApprovalRequestUnusedBalanceDto>> Handle(
        CalculateApprovalRequestUnusedBalanceCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => FetchCustomerNoAsync(ctx, cancellationToken))
            .Bind(ctx => TriggerAndAwaitBillProcessingAsync(ctx, cancellationToken))
            .Bind(ctx => FetchContractsAndSqlBalanceSimultaneouslyAsync(ctx, cancellationToken))
            .Map(CalculateFinalBalance);
    }

    private async Task<Result<PipelineContext>> FetchCustomerNoAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var resultDto = await creditProcessSqlService.GetCustomerNoByMosavabeNo(
            new GetCustomerNoByMosavabeNoDm(context.Command.MosavabeNo),
            cancellationToken);

        if (string.IsNullOrWhiteSpace(resultDto.CustomerNo))
        {
            return Result.Failure<PipelineContext>(
                new Error("Customer.NotFound", "شماره مشتری برای این مصوبه یافت نشد."));
        }

        return Result.Success(context with { CustomerNo = resultDto.CustomerNo });
    }

    private async Task<Result<PipelineContext>> TriggerAndAwaitBillProcessingAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        await loanBillProcessingService.InitiateLoanBillProcessing(
            context.CustomerNo!,
            context.Command.MosavabeNo,
            cancellationToken);

        await loanBillProcessingService.WaitForProcessingToCompleteOrTimeout(
            context.CustomerNo!,
            TimeSpan.FromSeconds(20),
            cancellationToken);

        return Result.Success(context);
    }

    private async Task<Result<PipelineContext>> FetchContractsAndSqlBalanceSimultaneouslyAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var mosavabeNo = context.Command.MosavabeNo;
        var cacheKey = $"ProcessedContractsByMosavabeNo_{mosavabeNo}";

        var cachedData = loanBillProcessingService.TryGetFromCacheByMosavabeNo(
            cacheKey, cancellationToken);

        var contractsTuple = cachedData ??
                             await loanBillProcessingService.GetProcessedContractsByMosavabeNo(
                                 mosavabeNo, cancellationToken);

        var mappedContracts = mapper.Map<List<GetLoanBillResultDm>>(contractsTuple.Item2);

        var sqlTask = creditProcessSqlService.CalculateApprovalRequestUnusedBalance(
            new CalculateApprovalRequestUnusedBalanceDm(mosavabeNo), cancellationToken);

        var settledAmount = creditProcessGeneralService.GetDailySettledContracts(
            new GetDailySettledContractsDm(mappedContracts), cancellationToken).SettledContractsAmounts;

        var receivedAmount = creditProcessGeneralService.GetDailyReceivedContractsTransactionsAmounts(
            new GetReceivedContractsDm(mappedContracts), cancellationToken).Amount;

        var (sqlAmount, remainingCount) = await sqlTask;

        return Result.Success(context with
        {
            DailySettledAmount = settledAmount,
            DailyReceivedAmount = receivedAmount,
            SqlBaseAmount = sqlAmount,
            RemainingContractsCount = Convert.ToInt32(remainingCount)
        });
    }

    private static CalculateApprovalRequestUnusedBalanceDto CalculateFinalBalance(PipelineContext context)
    {
        long baseAmount = 0;
        long receivedAmount = 0;

        if (!string.IsNullOrWhiteSpace(context.SqlBaseAmount))
            _ = long.TryParse(context.SqlBaseAmount, out baseAmount);

        if (!string.IsNullOrWhiteSpace(context.DailyReceivedAmount))
            _ = long.TryParse(context.DailyReceivedAmount, out receivedAmount);

        var finalCalculation = baseAmount + context.DailySettledAmount + receivedAmount;

        return new CalculateApprovalRequestUnusedBalanceDto(
            finalCalculation.ToString(),
            context.RemainingContractsCount);
    }
}
