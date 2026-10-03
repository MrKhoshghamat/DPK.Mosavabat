using DPK.Mosavabat.Common.Enums;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace DPK.Mosavabat.Services.CreditProcesses;

public class ContractTransactionService : IContractTransactionService
{
    private const string NormalIndicator = "0";
    private const string ReversedIndicator = "1";
    //private const string ReceivedStartDate = "050528";

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

        dataModel.Contracts?.RemoveAll(contract =>
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

    #region Public Methods (Overloads)

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش‌های وصولی بر اساس لیست مصوبات (GetMosavabeContractsDto)
    /// </summary>
    public GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(
        GetReceivedContractsDataModel dataModel,
        List<GetMosavabeContractsDto>? mosavabeContracts)
    {
        if (mosavabeContracts == null || !mosavabeContracts.Any())
            return new GetReceivedContractsDto { Amount = "0" };

        var contractRules = mosavabeContracts
            .Where(c => !string.IsNullOrWhiteSpace(c.C13))
            .Select(c => (ContractId: c.C13.Trim(), c.DateUpdate, c.TimeUpdate));

        return CalculateReceivedTransactionsCore(dataModel, contractRules);
    }

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش‌های وصولی بر اساس تسهیلات جاری مشتری (GetCustomerCurrentFacilityDto)
    /// </summary>
    public GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(
        GetReceivedContractsDataModel dataModel,
        List<GetCustomerCurrentFacilityDto>? customerCurrentFacilities)
    {
        if (customerCurrentFacilities == null || !customerCurrentFacilities.Any())
            return new GetReceivedContractsDto { Amount = "0" };

        var contractRules = customerCurrentFacilities
            .Select(f => (ContractId: f.FacilityNo.ToString(), f.DateUpdate, f.TimeUpdate));

        return CalculateReceivedTransactionsCore(dataModel, contractRules);
    }

    #endregion

    #region Core Calculation Engine

    /// <summary>
    /// متد هسته محاسبات مشترک جهت فیلتر و جمع مبالغ تراکنش‌ها در بازه اختصاصی هر قرارداد
    /// </summary>
    private GetReceivedContractsDto CalculateReceivedTransactionsCore(
        GetReceivedContractsDataModel dataModel,
        IEnumerable<(string ContractId, int? DateUpdate, int? TimeUpdate)>? contractRules)
    {
        if (!dataModel.LoanBills.Any() || contractRules == null)
        {
            return new GetReceivedContractsDto { Amount = "0" };
        }

        var rulesMap = contractRules
            .GroupBy(r => r.ContractId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        var nowKey = GetCurrentPersianDateTimeKey();

        long totalAmount = 0;

        foreach (var loanBill in dataModel.LoanBills.Where(loanBill => !string.IsNullOrWhiteSpace(loanBill.LoanId)))
        {
            if (!rulesMap.TryGetValue(loanBill.LoanId.Trim(), out var rule))
                continue;

            var startKey = CreateDateTimeKey(rule.DateUpdate, rule.TimeUpdate);

            if (!loanBill.Transactions.Any())
                continue;

            var billTransactionsSum = loanBill.Transactions
                .Where(t =>
                    IsWithinDateTimeRange(t.PaymentDate, t.PaymentTime, startKey, nowKey) &&
                    HasRequiredDescription(t) &&
                    IsNormalTransaction(t) &&
                    MatchesByCodeOrDescription(
                        t,
                        _transactionSettings.Received.Codes,
                        _transactionSettings.Received.Descriptions))
                .Sum(ParseAmount);

            totalAmount += billTransactionsSum;
        }

        return new GetReceivedContractsDto
        {
            Amount = totalAmount.ToString()
        };
    }

    #endregion

    #region Date & Time Helpers

    /// <summary>
    /// ایجاد کلید عددی 12 رقمی (yyyyMMddHHmm) از مقادیر DateUpdate و TimeUpdate
    /// </summary>
    private static long CreateDateTimeKey(int? dateUpdate, int? timeUpdate)
    {
        return (long)(dateUpdate * 10000L + timeUpdate)!;
    }

    /// <summary>
    /// دریافت تاریخ و زمان لحظه‌ای سیستم به صورت کلید عددی شمسی 12 رقمی yyyyMMddHHmm
    /// </summary>
    private static long GetCurrentPersianDateTimeKey()
    {
        var pc = new PersianCalendar();
        var now = DateTime.Now;

        var year = pc.GetYear(now);
        var month = pc.GetMonth(now);
        var day = pc.GetDayOfMonth(now);
        var hour = pc.GetHour(now);
        var minute = pc.GetMinute(now);

        return (year * 100000000L) +
               (month * 1000000L) +
               (day * 10000L) +
               (hour * 100L) +
               minute;
    }

    /// <summary>
    /// بررسی اینکه آیا تراکنش در بازه زمانی [startKey, endKey] قرار دارد یا خیر
    /// </summary>
    private static bool IsWithinDateTimeRange(string paymentDate, string paymentTime, long startKey, long endKey)
    {
        var transKey = ParseTransactionDateTimeKey(paymentDate, paymentTime);
        if (!transKey.HasValue)
            return false;

        return transKey.Value >= startKey && transKey.Value <= endKey;
    }

    /// <summary>
    /// تبدیل رشته تاریخ و زمان تراکنش به عدد استاندارد 12 رقمی yyyyMMddHHmm
    /// </summary>
    private static long? ParseTransactionDateTimeKey(string paymentDate, string paymentTime)
    {
        if (string.IsNullOrWhiteSpace(paymentDate))
            return null;

        var dateDigits = new string(paymentDate.Where(char.IsDigit).ToArray());

        if (dateDigits.Length == 6)
        {
            dateDigits = "14" + dateDigits;
        }

        if (dateDigits.Length != 8)
            return null;

        var timeDigits = string.IsNullOrWhiteSpace(paymentTime)
            ? "0000"
            : new string(paymentTime.Where(char.IsDigit).ToArray());

        switch (timeDigits.Length)
        {
            case < 4:
                timeDigits = timeDigits.PadLeft(4, '0');
                break;
            case > 4:
                timeDigits = timeDigits[..4];
                break;
        }

        if (long.TryParse(dateDigits + timeDigits, out var fullKey))
        {
            return fullKey;
        }

        return null;
    }

    #endregion

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
                        IsSameDate(t.PaymentDate, todayFormattedDate) &&
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

    //private static bool IsInDateRange(string? value, string fromDate, string toDate)
    //{
    //    return !string.IsNullOrWhiteSpace(value) &&
    //           string.Compare(value, fromDate, StringComparison.Ordinal) >= 0 &&
    //           string.Compare(value, toDate, StringComparison.Ordinal) <= 0;
    //}

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
