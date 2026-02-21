using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Services.LoanBills.LoanBillProcessing.Contract;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.GeneralServices.CreditProcesses.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CalculateCustomerTotalDebt;

/// <summary>
/// هندلر محاسبه مانده بدهی کل مشتری
/// </summary>
internal class CalculateCustomerTotalDebtCommandHandler(
    ILoanBillProcessingService loanBillProcessingService,
    ICreditProcessGeneralService creditProcessGeneralService,
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : ICommandHandler<
        CalculateCustomerTotalDebtCommand,
        Result<CalculateCustomerTotalDebtDto>>
{
    private record PipelineContext(
        CalculateCustomerTotalDebtCommand Command,
        object? RawCurrentFacilities = null,
        object? RawLoanBills = null,
        long SqlDailyPaidContractsSum = 0,
        long TotalSettledDebtSum = 0,
        long DailyPaidAmountsSum = 0);

    public async Task<Result<CalculateCustomerTotalDebtDto>> Handle(
        CalculateCustomerTotalDebtCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => ProcessAndFetchAllDataAsync(ctx, cancellationToken))
            .Map(PerformCpuBoundCalculations)
            .Map(CalculateFinalDebt);
    }

    private async Task<Result<PipelineContext>> ProcessAndFetchAllDataAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var customerNo = context.Command.CustomerNo;

        var sqlTask = creditProcessSqlService.CalculateDailyPaidContractsAmountsSum(
            new CalculateDailyPaidContractsAmountsSumDm(customerNo), cancellationToken);

        await loanBillProcessingService.InitiateLoanBillProcessing(customerNo, cancellationToken);

        await loanBillProcessingService.WaitForProcessingToCompleteOrTimeout(
            customerNo, TimeSpan.FromSeconds(20), cancellationToken);

        var cacheKey = $"ProcessedContractsByCustomerNo_{customerNo}";
        var cached = loanBillProcessingService
            .TryGetFromCacheByCustomerNo(
                cacheKey,
                cancellationToken);

        var contractsTuple = cached ??
                             await loanBillProcessingService.GetProcessedContractsByCustomerNo(
                                 customerNo, cancellationToken);

        var sqlDto = await sqlTask;
        var sqlDailySum = sqlDto.DailyPaidContractsAmountsSum;

        return Result.Success(context with
        {
            RawCurrentFacilities = contractsTuple.Item1,
            RawLoanBills = contractsTuple.Item2,
            SqlDailyPaidContractsSum = sqlDailySum
        });
    }

    private PipelineContext PerformCpuBoundCalculations(PipelineContext context)
    {
        var mappedLoanBills = mapper.Map<List<GetLoanBillResultDm>>(context.RawLoanBills);
        var mappedFacilities = mapper.Map<List<GetCustomerCurrentFacilityDm>>(context.RawCurrentFacilities);

        var settledContracts = creditProcessGeneralService.RemoveSettledContracts(
            new RemoveSettledContractsDm(mappedLoanBills, mappedFacilities),
            default).Contracts;

        var totalSettledDebtSum = settledContracts?.Sum(facility => facility.TotalDebtBalance) ?? 0;

        var dailyPaidAmountsStr = creditProcessGeneralService.GetDailyPaidContractsTransactionsAmounts(
            new GetDailyPaidContractsTransactionsAmountsDm(mappedLoanBills),
            default).Amount;

        long dailyPaidAmountsSum = 0;
        if (!string.IsNullOrWhiteSpace(dailyPaidAmountsStr))
        {
            _ = long.TryParse(dailyPaidAmountsStr, out dailyPaidAmountsSum);
        }

        return context with
        {
            TotalSettledDebtSum = totalSettledDebtSum,
            DailyPaidAmountsSum = dailyPaidAmountsSum
        };
    }

    private static CalculateCustomerTotalDebtDto CalculateFinalDebt(
        PipelineContext context)
    {
        var totalCurrentDebtAmount =
            context.TotalSettledDebtSum +
            context.DailyPaidAmountsSum +
            context.SqlDailyPaidContractsSum -
            context.Command.ReceivedDailyPaymentAmountsSum;

        return new CalculateCustomerTotalDebtDto(totalCurrentDebtAmount);
    }
}
