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

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.AggregateDailyCustomerPayment;

/// <summary>
/// هندلر سرویس جمع پرداختی های روز مشتری
/// </summary>
internal class AggregateDailyCustomerPaymentCommandHandler(
    ILoanBillProcessingService loanBillProcessingService,
    ICreditProcessGeneralService creditProcessGeneralService,
    IMapper mapper)
    : ICommandHandler<
        AggregateDailyCustomerPaymentCommand,
        AggregateDailyCustomerPaymentDto>
{
    public async Task<AggregateDailyCustomerPaymentDto> Handle(
        AggregateDailyCustomerPaymentCommand command,
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

        var dailyReceivedContractsAmounts = creditProcessGeneralService
            .GetDailyReceivedContractsTransactionsAmounts(
                new GetReceivedContractsDm(
                    loanBills: mapper.Map<List<GetLoanBillResultDm>>(contracts.Item2)),
                cancellationToken).Amount;

        return new AggregateDailyCustomerPaymentDto(
            dailyReceivedContractsAmounts);
    }
}
