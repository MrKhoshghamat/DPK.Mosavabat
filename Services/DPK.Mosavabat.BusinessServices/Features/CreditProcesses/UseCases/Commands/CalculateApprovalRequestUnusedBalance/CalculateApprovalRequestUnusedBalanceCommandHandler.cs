using System.Collections.Generic;
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

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CalculateApprovalRequestUnusedBalance;

/// <summary>
/// هندلر سرویس محاسبه مانده آزاد مصوبه
/// </summary>
internal class CalculateApprovalRequestUnusedBalanceCommandHandler(
    ICreditProcessSqlService creditProcessSqlService,
    ILoanBillProcessingService loanBillProcessingService,
    ICreditProcessGeneralService creditProcessGeneralService,
    IMapper mapper)
    : ICommandHandler<CalculateApprovalRequestUnusedBalanceCommand,
        CalculateApprovalRequestUnusedBalanceDto>
{
    public async Task<CalculateApprovalRequestUnusedBalanceDto> Handle(
        CalculateApprovalRequestUnusedBalanceCommand command,
        CancellationToken cancellationToken)
    {
        var getCustomerNoByMosavabeNoDto =
            await creditProcessSqlService
                .GetCustomerNoByMosavabeNo(
                    new GetCustomerNoByMosavabeNoDm(command.MosavabeNo),
                    cancellationToken);

        await loanBillProcessingService
            .InitiateLoanBillProcessing(
                getCustomerNoByMosavabeNoDto.CustomerNo,
                command.MosavabeNo,
                cancellationToken);

        await loanBillProcessingService
            .WaitForProcessingToCompleteOrTimeout(
                getCustomerNoByMosavabeNoDto.CustomerNo,
                TimeSpan.FromSeconds(20),
                cancellationToken);

        var cacheKey = $"ProcessedContractsByMosavabeNo_{command.MosavabeNo}";
        var cached = loanBillProcessingService.TryGetFromCacheByMosavabeNo(
            cacheKey,
            cancellationToken);

        var contracts = cached ?? await loanBillProcessingService
            .GetProcessedContractsByMosavabeNo(
                command.MosavabeNo,
                cancellationToken);

        var dailySettledContractsAmountsSum = creditProcessGeneralService
            .GetDailySettledContracts(new GetDailySettledContractsDm(
                    mapper.Map<List<GetLoanBillResultDm>>(contracts.Item2)),
                cancellationToken)
            .SettledContractsAmounts;

        var dailyReceivedAmountSum = creditProcessGeneralService
            .GetDailyReceivedContractsTransactionsAmounts(new GetReceivedContractsDm(
                    mapper.Map<List<GetLoanBillResultDm>>(contracts.Item2)),
                cancellationToken)
            .Amount;

        var (amount, remainingContractsCount) = await creditProcessSqlService
            .CalculateApprovalRequestUnusedBalance(new CalculateApprovalRequestUnusedBalanceDm(
                    command.MosavabeNo),
                cancellationToken);

        var finalCalculation =
            Convert.ToInt64(amount) +
            dailySettledContractsAmountsSum +
            Convert.ToInt64(dailyReceivedAmountSum);

        return new CalculateApprovalRequestUnusedBalanceDto(
            finalCalculation.ToString(),
            remainingContractsCount);
    }
}