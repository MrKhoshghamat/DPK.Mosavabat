using System.Collections.Generic;
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

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.AggregateDailyCustomerPayment;

/// <summary>
/// هندلر تجمیع پرداختی‌های روز مشتری
/// </summary>
internal class AggregateDailyCustomerPaymentCommandHandler(
    ILoanBillProcessingService loanBillProcessingService,
    ICreditProcessGeneralService creditProcessGeneralService,
    IMapper mapper)
    : ICommandHandler<
        AggregateDailyCustomerPaymentCommand,
        Result<AggregateDailyCustomerPaymentDto>>
{
    private record PipelineContext(
        AggregateDailyCustomerPaymentCommand Command,
        IEnumerable<object>? RawContracts = null);

    public async Task<Result<AggregateDailyCustomerPaymentDto>> Handle(
        AggregateDailyCustomerPaymentCommand command,
        CancellationToken cancellationToken)
    {
        return await Result.Success(new PipelineContext(command))
            .Bind(ctx => TriggerAndAwaitBillProcessingAsync(ctx, cancellationToken))
            .Bind(ctx => FetchProcessedContractsAsync(ctx, cancellationToken))
            .Map(CalculateDailyPayment);
    }

    private async Task<Result<PipelineContext>> TriggerAndAwaitBillProcessingAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var customerNo = context.Command.CustomerNo;

        await loanBillProcessingService.InitiateLoanBillProcessing(
            customerNo,
            cancellationToken);

        await loanBillProcessingService.WaitForProcessingToCompleteOrTimeout(
            customerNo,
            TimeSpan.FromSeconds(20),
            cancellationToken);

        return Result.Success(context);
    }

    private async Task<Result<PipelineContext>> FetchProcessedContractsAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var customerNo = context.Command.CustomerNo;
        var cacheKey = $"ProcessedContractsByCustomerNo_{customerNo}";

        var cachedData = loanBillProcessingService.TryGetFromCacheByCustomerNo(
            cacheKey,
            cancellationToken);

        var contractsTuple = cachedData ??
                             await loanBillProcessingService.GetProcessedContractsByCustomerNo(
                                 customerNo,
                                 cancellationToken);

        return Result.Success(context with { RawContracts = contractsTuple.Item2 });
    }

    private AggregateDailyCustomerPaymentDto CalculateDailyPayment(PipelineContext context)
    {
        var mappedLoanBills = mapper.Map<List<GetLoanBillResultDm>>(context.RawContracts);

        var result = creditProcessGeneralService.GetDailyReceivedContractsTransactionsAmounts(
            new GetReceivedContractsDm(loanBills: mappedLoanBills),
            CancellationToken.None);

        return new AggregateDailyCustomerPaymentDto(result.Amount);
    }
}
