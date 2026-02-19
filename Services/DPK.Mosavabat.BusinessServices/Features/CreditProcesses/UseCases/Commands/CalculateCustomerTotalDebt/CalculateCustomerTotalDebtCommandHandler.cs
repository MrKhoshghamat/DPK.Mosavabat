using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Services.LoanBills.LoanBillProcessing.Contract;
using DPK.Mosavabat.GeneralServices.CreditProcesses.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CalculateCustomerTotalDebt;

/// <summary>
/// هندلر سرویس محاسبه مانده بدهی کل مشتری
/// </summary>
internal class CalculateCustomerTotalDebtCommandHandler(
    ILoanBillProcessingService loanBillProcessingService,
    ICreditProcessGeneralService creditProcessGeneralService,
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : ICommandHandler<
        CalculateCustomerTotalDebtCommand,
        CalculateCustomerTotalDebtDto>
{
    public async Task<CalculateCustomerTotalDebtDto> Handle(
        CalculateCustomerTotalDebtCommand command,
        CancellationToken cancellationToken)
    {
        await loanBillProcessingService
            .InitiateLoanBillProcessing(
                command.CustomerNo,
                cancellationToken);

        await loanBillProcessingService
            .WaitForProcessingToCompleteOrTimeout(
                command.CustomerNo,
                TimeSpan.FromSeconds(20),
                cancellationToken);

        var cacheKey = $"ProcessedContractsByCustomerNo_{command.CustomerNo}";
        var cached = loanBillProcessingService
            .TryGetFromCacheByCustomerNo(
                cacheKey,
                cancellationToken);

        var contracts = cached
                        ?? await loanBillProcessingService
                            .GetProcessedContractsByCustomerNo(
                                command.CustomerNo,
                                cancellationToken);

        var settledContracts = creditProcessGeneralService
            .RemoveSettledContracts(
                new RemoveSettledContractsDm(
                    mapper.Map<List<GetLoanBillResultDm>>(contracts.Item2),
                    mapper.Map<List<GetCustomerCurrentFacilityDm>>(contracts.Item1)),
                cancellationToken)
            .Contracts;

        var dailyPaidAmounts = creditProcessGeneralService
            .GetDailyPaidContractsTransactionsAmounts(
                new GetDailyPaidContractsTransactionsAmountsDm(
                    mapper.Map<List<GetLoanBillResultDm>>(contracts.Item2)),
                cancellationToken)
            .Amount;

        var dailyPaidContractsAmountsSumDto = await creditProcessSqlService
            .CalculateDailyPaidContractsAmountsSum(
                new CalculateDailyPaidContractsAmountsSumDm(
                    command.CustomerNo),
                cancellationToken);

        var totalCurrentDebtAmount = settledContracts
                                         .Sum(facility => facility.TotalDebtBalance) +
                                     long.Parse(dailyPaidAmounts) +
                                     dailyPaidContractsAmountsSumDto.DailyPaidContractsAmountsSum -
                                     command.ReceivedDailyPaymentAmountsSum;

        return new CalculateCustomerTotalDebtDto(
            totalCurrentDebtAmount);
    }
}