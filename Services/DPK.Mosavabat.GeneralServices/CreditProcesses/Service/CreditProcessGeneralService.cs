using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DPK.Mosavabat.Common.Enums;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.GeneralServices.CreditProcesses.Contract;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.LoanBills;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.GeneralServices.CreditProcesses.Service;

public class CreditProcessGeneralService
    : ICreditProcessGeneralService
{
    public GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(
        GetReceivedContractsDm dm,
        CancellationToken cancellationToken)
    {
        var todayFormattedDate = DateTime.Now
            .ToFormattedPersianDate(
                DateFormat.TwoDigitYear);

        List<long> transactionsAmount;

        var hasTransactionCode = dm.LoanBills
            .Any(lb => lb.Transactions
                .Any(t => !string.IsNullOrEmpty(t.TransactionCode)));

        if (hasTransactionCode)
        {
            transactionsAmount = (
                from loanBill in dm.LoanBills
                select loanBill.Transactions.Where(t =>
                    t.PaymentDate == todayFormattedDate && t is
                    {
                        TransactionCode:
                        "00BA" or // وصول قسط امهال
                        "00BH" or // وصول تدریجی مطالبات با احکام دادگاه
                        "00BI" or // وصول مانده مطالبات با احکام دادگاه
                        "00B4" or // وصول تدریجی مطالبات تسهیلات قسطی
                        "00B5" or // وصول تدریجی مطالبات غیر قسطی
                        "00B6" or // وصول مانده مطالبات تسهبلات قسطی
                        "00B7" or // وصول مانده مطالبات غیر قسطی
                        "00H2" or // وصول قسط امهال در سر رسید گذشته و معوق
                        "0013" or // کاهش سرمایه
                        "0014" or // وصول قسط
                        "0015" or // بازپرداخت تدریجی
                        "0018" or // وصول بعد از سر رسید
                        "0065" or // وصول مرحله‌ای
                        "0079" or // وصول تدریجی مطالبات
                        "0080"    // وصول مطالبات بانک
                    }).ToList()
                into transactions
                let normalTransactions = transactions.Where(x => x.Indicator == "0").ToList()
                let reversedTransactions = transactions.Where(x => x.Indicator == "1").ToList()
                let normalTransactionsAmountSum = normalTransactions.Sum(x => long.Parse(x.Amount))
                let reversedTransactionsAmountSum = reversedTransactions.Sum(x => long.Parse(x.Amount))
                select normalTransactionsAmountSum - reversedTransactionsAmountSum).ToList();
        }
        else
        {
            transactionsAmount = (
                from loanBill in dm.LoanBills
                select loanBill.Transactions.Where(t =>
                    t.PaymentDate == todayFormattedDate && t is
                    {
                        Description:
                        "وصول قسط امهال" or
                        "وصولتدريجيمطالبات باحكم دادگاه" or
                        "وصولمانده مطالبات باحكم دادگاه" or
                        "وصولتدريجيمطالبات تسهيلات قسطي" or
                        "وصول تدريجي مطالبات غيرقسطي" or
                        "وصولماندهمطالبات تسهيلات قسطي" or
                        "وصول مانده مطالبات غير قسطي" or
                        "وصولقسطامهالدرسررسيدگذشتهمعوق" or
                        "كاهش سرمايه" or
                        "وصول قسط" or
                        "وصول‌ قسط" or
                        "بازپرداخت تدريجي" or
                        "وصول بعد از سررسيد" or
                        "وصول مرحله اي" or
                        "وصولتدريجيمطالبات" or
                        "وصولمطالباتبانك"
                    }).ToList()
                into transactions
                let normalTransactions = transactions.Where(x => x.Indicator == "0").ToList()
                let reversedTransactions = transactions.Where(x => x.Indicator == "1").ToList()
                let normalTransactionsAmountSum = normalTransactions.Sum(x => long.Parse(x.Amount))
                let reversedTransactionsAmountSum = reversedTransactions.Sum(x => long.Parse(x.Amount))
                select normalTransactionsAmountSum - reversedTransactionsAmountSum).ToList();
        }

        return new GetReceivedContractsDto(
            transactionsAmount
                .Sum()
                .ToString());
    }

    public GetDailyPaidContractsTransactionsAmountsDto GetDailyPaidContractsTransactionsAmounts(
        GetDailyPaidContractsTransactionsAmountsDm dm,
        CancellationToken cancellationToken)
    {
        var todayFormattedDate = DateTime.Now
            .ToFormattedPersianDate(
                DateFormat.TwoDigitYear);

        List<long> transactionsAmount;

        var hasTransactionCode = dm.LoanBills
            .Any(lb => lb.Transactions
                .Any(t => !string.IsNullOrEmpty(t.TransactionCode)));

        if (hasTransactionCode)
        {
            transactionsAmount = (
                from loanBill in dm.LoanBills
                select loanBill.Transactions.Where(t =>
                    t.PaymentDate == todayFormattedDate && t is
                    {
                        TransactionCode:
                        "0005" or // پرداخت تسهیلات
                        "0007" or // پیش پرداخت
                        "0012" or // افزایش سرمایه
                        "0036" or // فروش نقدی کالای سلف
                        "0039"    // پرداخت سرمایه مشارکت مدنی
                    }).ToList()
                into transactions
                let normalTransactions = transactions.Where(x => x.Indicator == "0").ToList()
                let reversedTransactions = transactions.Where(x => x.Indicator == "1").ToList()
                let normalTransactionsAmountSum = normalTransactions.Sum(x => long.Parse(x.Amount))
                let reversedTransactionsAmountSum = reversedTransactions.Sum(x => long.Parse(x.Amount))
                select normalTransactionsAmountSum - reversedTransactionsAmountSum).ToList();
        }
        else
        {
            transactionsAmount = (
                from loanBill in dm.LoanBills
                select loanBill.Transactions.Where(t =>
                    t.PaymentDate == todayFormattedDate && t is
                    {
                        Description:
                        "پرداخت تسهيلات" or
                        "پيش پرداخت" or
                        "افزايش سرمايه" or
                        "فروش نقدي كالاي سلف" or
                        "پرداخت سرمايهمشاركت مدني" or
                        "پرد‌اخت‌ سرمایه‌مشارکت‌ مدنی‌"
                    }).ToList()
                into transactions
                let normalTransactions = transactions.Where(x => x.Indicator == "0").ToList()
                let reversedTransactions = transactions.Where(x => x.Indicator == "1").ToList()
                let normalTransactionsAmountSum = normalTransactions.Sum(x => long.Parse(x.Amount))
                let reversedTransactionsAmountSum = reversedTransactions.Sum(x => long.Parse(x.Amount))
                select normalTransactionsAmountSum - reversedTransactionsAmountSum).ToList();
        }

        return new GetDailyPaidContractsTransactionsAmountsDto(
            transactionsAmount
                .Sum()
                .ToString());
    }

    public GetDailySettledContractsDto GetDailySettledContracts(
        GetDailySettledContractsDm dm,
        CancellationToken cancellationToken)
    {
        var todayFormattedDate = DateTime.Now
            .ToFormattedPersianDate(
                DateFormat.TwoDigitYear);

        List<long> transactionsAmountsList;

        var hasTransactionCode = dm.LoanBills
            .Any(lb => lb.Transactions
                .Any(t => !string.IsNullOrEmpty(t.TransactionCode)));

        if (hasTransactionCode)
        {
            transactionsAmountsList = (
                from loanBill in dm.LoanBills
                select loanBill.Transactions.Where(t =>
                    t.PaymentDate == todayFormattedDate && t is
                    {
                        Indicator: "0",
                        TransactionCode:
                        "00BB" or // تسویه نهایی امهال
                        "00H3" or // تسویه امهال در سر رسید گذشته و معوق
                        "0024" or // تسویه بعد از سر رسید
                        "0016"    // تسویه
                    }).ToList()
                into transactions
                let transactionsList = transactions.ToList()
                let transactionsAmounts = transactionsList.Sum(x => long.Parse(x.Amount))
                select transactionsAmounts).ToList();
        }
        else
        {
            transactionsAmountsList = (
                from loanBill in dm.LoanBills
                select loanBill.Transactions.Where(t =>
                    t.PaymentDate == todayFormattedDate && t is
                    {
                        Indicator: "0",
                        Description:
                        "تسويه نهايي امهال" or
                        "تسويهامهال درسررسيدگذشته ومعوق" or
                        "تسويه تسهيلات تضمين دولت" or
                        "تسويه بعدازسررسيد" or
                        "تسویه‌بعد‌ازسررسید" or
                        "تسويه" or
                        "تسویه‌"
                    }).ToList()
                into transactions
                let transactionsList = transactions.ToList()
                let transactionsAmounts = transactionsList.Sum(x => long.Parse(x.Amount))
                select transactionsAmounts).ToList();
        }

        return new GetDailySettledContractsDto(
            transactionsAmountsList
                .Sum());
    }

    public RemoveSettledContractsDto RemoveSettledContracts(
        RemoveSettledContractsDm dm,
        CancellationToken cancellationToken)
    {
        List<GetLoanBillResultDm> settledLoans;

        var hasTransactionCode = dm.LoanBills
        .Any(lb => lb.Transactions
                .Any(t => !string.IsNullOrEmpty(t.TransactionCode)));

        if (hasTransactionCode)
        {
            settledLoans = dm.LoanBills
                .Where(x =>
                    x.Transactions.Any(t =>
                        t is
                        {
                            Indicator: "0",
                            TransactionCode:
                            "00BB" or // تسویه نهایی امهال
                            "00H3" or // تسویه امهال در سر رسید گذشته و معوق
                            "0024" or // تسویه بعد از سر رسید
                            "0016"    // تسویه
                        })).ToList();
        }
        else
        {
            settledLoans = dm.LoanBills
                .Where(x =>
                    x.Transactions.Any(t =>
                        t is
                        {
                            Indicator: "0",
                            Description:
                            "تسويه نهايي امهال" or
                            "تسويهامهال درسررسيدگذشته ومعوق" or
                            "تسويه تسهيلات تضمين دولت" or
                            "تسويه بعدازسررسيد" or
                            "تسویه‌بعد‌ازسررسید" or
                            "تسويه" or
                            "تسویه‌"
                        })).ToList();
        }

        foreach (var settledLoan in from settledLoan in settledLoans
                     .ToList()
                                    let transactions = settledLoan.Transactions
                                        .OrderBy(x => x.PaymentDate)
                                        .ThenBy(x => x.PaymentTime)
                                        .ToList()
                                    let firstTransactions = transactions
                                        .FirstOrDefault()
                                    where firstTransactions.Indicator == "1"
                                    select settledLoan)
        {
            settledLoans.Remove(settledLoan);
        }

        var settledLoanBillsKeys = settledLoans.Select(x => x.LoanId).ToHashSet();

        foreach (var unused in dm.Contracts.ToList())
        {
            dm.Contracts.RemoveAll(facility => settledLoanBillsKeys.Contains(facility.FacilityNo.ToString()));
        }

        return new RemoveSettledContractsDto(dm.Contracts.Select(getCustomerCurrentFacilityDm =>
                new CustomerCurrentFacilityDto(
                    getCustomerCurrentFacilityDm.CustomerNo,
                    getCustomerCurrentFacilityDm.FacilityNo,
                    getCustomerCurrentFacilityDm.FacilityStatusCode,
                    getCustomerCurrentFacilityDm.CurrentInstallmentBalance,
                    getCustomerCurrentFacilityDm.NonCurrentInstallmentBalance,
                    getCustomerCurrentFacilityDm.TotalDebtBalance))
            .ToList());
    }
}