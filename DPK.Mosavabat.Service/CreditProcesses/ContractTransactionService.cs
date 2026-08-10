using DPK.Mosavabat.Common.Enums;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using Microsoft.Extensions.Options;

namespace DPK.Mosavabat.Services.CreditProcesses;

public class ContractTransactionService : IContractTransactionService
{
    private const string NormalIndicator = "0";
    private const string ReversedIndicator = "1";
    private const string ReceivedStartDate = "050503";

    private readonly TransactionSettings _transactionSettings;

    public ContractTransactionService(IOptions<TransactionSettings> transactionSettingsOptions)
    {
        _transactionSettings = transactionSettingsOptions.Value;
    }

    /// <summary>
    /// حذف قراردادهای تسویه شده از لیست قراردادها
    /// </summary>
    public RemoveSettleContractsDto RemoveSettledContracts(RemoveSettleContractsDataModel dataModel)
    {
        var settledLoanIds = GetSettledLoanIds(dataModel.LoanBills).ToHashSet();

        dataModel.Contracts.RemoveAll(contract =>
            settledLoanIds.Contains(contract.FacilityNo.ToString()));

        return new RemoveSettleContractsDto
        {
            Contracts = dataModel.Contracts
        };
    }

    /// <summary>
    /// دریافت قراردادهای تسویه شده
    /// </summary>
    public GetSettledContractsDto GetSettledContracts(GetSettledContractsDataModel dataModel)
    {
        var settledLoanIds = GetSettledLoanIds(dataModel.LoanBills)
            .Distinct()
            .ToList();

        return new GetSettledContractsDto
        {
            C13List = settledLoanIds
        };
    }

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش های وصولی قراردادها
    /// </summary>
    public GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(GetReceivedContractsDataModel dataModel)
    {
        var todayFormattedDate = DateTime.Now.ToFormattedPersianDate(DateFormat.TwoDigitYear);

        var amount = dataModel.LoanBills
            .Select(loanBill => loanBill.Transactions
                .Where(t =>
                    IsInDateRange(t.PaymentDate, ReceivedStartDate, todayFormattedDate) &&
                    HasRequiredDescription(t) &&
                    IsNormalTransaction(t) &&
                    MatchesByCodeOrDescription(
                        t,
                        _transactionSettings.Received.Codes,
                        _transactionSettings.Received.Descriptions))
                .Sum(ParseAmount))
            .Sum();

        return new GetReceivedContractsDto
        {
            Amount = amount.ToString()
        };
    }

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش های پرداختی قراردادها
    /// </summary>
    public GetPaidContractsDto GetDailyPaidContractsTransactionsAmounts(GetPaidContractsDataModel dataModel)
    {
        var todayFormattedDate = DateTime.Now.ToFormattedPersianDate(DateFormat.TwoDigitYear);

        var amount = dataModel.LoanBills
            .Select(loanBill => loanBill.Transactions
                .Where(t =>
                    IsSameDate(t.PaymentDate, todayFormattedDate) &&
                    MatchesByCodeOrDescription(
                        t,
                        _transactionSettings.Paid.Codes,
                        _transactionSettings.Paid.Descriptions))
                .ToList())
            .Select(CalculateNetAmount)
            .Sum();

        return new GetPaidContractsDto
        {
            Amount = amount.ToString()
        };
    }

    public List<GetReceivedAllContractsDto> GetDailyReceivedAllContractsTransactionsAmounts(
        GetReceivedAllContractsDataModel dataModel)
    {
        var todayFormattedDate = DateTime.Now.ToFormattedPersianDate(DateFormat.TwoDigitYear);

        return dataModel.LoanBills
            .Select(loanBill =>
            {
                var amount = loanBill.Transactions
                    .Where(t =>
                        IsInDateRange(t.PaymentDate, ReceivedStartDate, todayFormattedDate) &&
                        HasRequiredDescription(t) &&
                        IsNormalTransaction(t) &&
                        MatchesByCodeOrDescription(
                            t,
                            _transactionSettings.Received.Codes,
                            _transactionSettings.Received.Descriptions))
                    .Sum(ParseAmount);

                return new GetReceivedAllContractsDto
                {
                    C13 = loanBill.LoanId,
                    Amount = amount.ToString()
                };
            })
            .Where(x => x.Amount != "0")
            .ToList();
    }

    public RemoveSettleAllContractsDto RemoveSettledAllContracts(RemoveSettleAllContractsDataModel dataModel)
    {
        var settledLoanIds = GetSettledLoanIds(dataModel.LoanBills).ToHashSet();

        dataModel.Contracts.RemoveAll(contract =>
            settledLoanIds.Contains(contract.C13.ToString()));

        return new RemoveSettleAllContractsDto
        {
            Contracts = dataModel.Contracts
        };
    }

    private IEnumerable<string> GetSettledLoanIds(IEnumerable<GetLoanBillDm> loanBills)
    {
        return loanBills
            .Where(HasValidSettledTransaction)
            .Where(DoesNotStartWithReversedTransaction)
            .Select(lb => lb.LoanId);
    }

    private bool HasValidSettledTransaction(GetLoanBillDm loanBill)
    {
        return loanBill.Transactions.Any(t =>
            IsNormalTransaction(t) &&
            MatchesByCodeOrDescription(
                t,
                _transactionSettings.Settled.Codes,
                _transactionSettings.Settled.Descriptions));
    }

    private static bool DoesNotStartWithReversedTransaction(GetLoanBillDm loanBill)
    {
        var firstTransaction = loanBill.Transactions
            .OrderBy(x => x.PaymentDate)
            .ThenBy(x => x.PaymentTime)
            .FirstOrDefault();

        return firstTransaction?.Indicator != ReversedIndicator;
    }

    private static bool HasRequiredDescription(TransactionDm transaction)
    {
        return !string.IsNullOrWhiteSpace(transaction.Description);
    }

    private static bool IsNormalTransaction(TransactionDm transaction)
    {
        return transaction.Indicator == NormalIndicator;
    }

    private static bool IsSameDate(string? value, string targetDate)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               string.Equals(value, targetDate, StringComparison.Ordinal);
    }

    private static bool IsInDateRange(string? value, string fromDate, string toDate)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               string.Compare(value, fromDate, StringComparison.Ordinal) >= 0 &&
               string.Compare(value, toDate, StringComparison.Ordinal) <= 0;
    }

    private static long CalculateNetAmount(IEnumerable<TransactionDm> transactions)
    {
        var transactionDms = transactions as TransactionDm[] ?? transactions.ToArray();
        var normalSum = transactionDms
            .Where(x => x.Indicator == NormalIndicator)
            .Sum(ParseAmount);

        var reversedSum = transactionDms
            .Where(x => x.Indicator == ReversedIndicator)
            .Sum(ParseAmount);

        return normalSum - reversedSum;
    }

    private static long ParseAmount(TransactionDm transaction)
    {
        return long.TryParse(transaction.Amount, out var value) ? value : 0;
    }

    private static bool MatchesByCodeOrDescription(
        TransactionDm transaction,
        IEnumerable<string> codes,
        IEnumerable<string> descriptions)
    {
        if (!string.IsNullOrWhiteSpace(transaction.TransactionCode))
        {
            return codes.Contains(transaction.TransactionCode);
        }

        return !string.IsNullOrWhiteSpace(transaction.Description) &&
               descriptions.Contains(transaction.Description);
    }
}
