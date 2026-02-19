using DPK.Mosavabat.Common.Enums.CreditProcesses;
using System.Collections.Generic;

namespace DPK.Mosavabat.BusinessServices.Abstractions.Errors;


/// <summary>
/// پرووایدر تشریح خطاهای بیزینسی
/// </summary>
public class CreditProcessMessageProvider 
    : ICreditProcessMessageProvider
{
    private readonly Dictionary<CreditProcessErrors, string> _messages;

    /// <summary>
    /// Constructor
    /// </summary>
    public CreditProcessMessageProvider()
    {
        _messages = new Dictionary<CreditProcessErrors, string>
        {
            { CreditProcessErrors.Unauthorized, "مجوز ارتباط با سرویس یافت نشد" },
            { CreditProcessErrors.OperationFailed, "خطا در عملیات ثبت قرارداد تکمیل اطلاعات در مصوبات" },
            { CreditProcessErrors.InvalidParameter, "پارامتر نامعتبر می باشد" },
            { CreditProcessErrors.ParameterRequired, "پارامتر الزامی می باشد" },
            { CreditProcessErrors.SystemError, "سیستم با مشکل مواجه شده است لطفا با مسئول سیستم تماس بگیرید" },
            { CreditProcessErrors.MosavabeNotFound, "مصوبه يافت نشد" },
            { CreditProcessErrors.ContractAmountExceedsMosavabe, "مبلغ قرارداد نمي تواند بزرگتر از مبلغ مصوبه باشد" },
            { CreditProcessErrors.PrepayAmountLessThanMosavabe, "مبلغ پيش دریافت قرارداد نمی تواند کمتر از مبلغ پیش دریافت مصوبه باشد" },
            { CreditProcessErrors.MosavabeExpired, "مصوبه منقضی شده است" },
            { CreditProcessErrors.GroupMosavabeLimitReached, "تعداد قرارداد هاي مصوبه گروهی به اتمام رسیده است" },
            { CreditProcessErrors.AverageNotMaintained, "معدل حفظ نشده است" },
            { CreditProcessErrors.CustomerHasBouncedCheck, "مشتری چک برگشتی دارد" },
            { CreditProcessErrors.PendingPermissionExists, "برای این مصوبه مجوز تعیین تکلیف نشده وجود دارد ابتدا برای مجوز قبلی قرارداد ثبت نمایید سپس اقدام به دریافت مجوز جدید نمایید" },
            { CreditProcessErrors.DuplicatePermissionNotAllowed, "درج مجوز تکراری مجاز نیست" },
            { CreditProcessErrors.PendingMosavabePermissionExists, "برای مصوبه مجوز تعیین تکلیف نشده وجود دارد امکان صدور مجوز وجود ندارد" },
            { CreditProcessErrors.MosavabeMatured, "مصوبه سر رسید شده است" },
            { CreditProcessErrors.MosavabeNoFreeBalance, "مصوبه مانده آزاد ندارد" },
            { CreditProcessErrors.MultiplePaymentPermissionsFound, "بیش از یک مجوز پرداخت برای تسهیلات مورد نظر وجود دارد" },
            { CreditProcessErrors.PaymentPermissionNotFound, "مجوز پرداخت برای تسهیلات مورد نظر یافت نشد" },
            { CreditProcessErrors.DuplicatePaymentPermissionForFacility, "مجوز پرداخت برای تسهیلات مورد نظر تکراری است" },
            { CreditProcessErrors.PendingPaymentPermissionForCustomer, "یک مجوز پرداخت تعیین تکلیف نشده برای این شماره مشتری وجود دارد" },
            { CreditProcessErrors.DeleteForbiddenHasContract, "شماره قرارداد وجود دارد، لذا قادر به حذف مجوز پرداخت نیستید" },
            { CreditProcessErrors.CancelForbiddenNoContract, "شماره قرارداد وجود ندارد، لذا قادر به ابطال مجوز پرداخت نیستید" },
            { CreditProcessErrors.NotFoundPaymentPermission, "شماره قرارداد یافت نشد" },
            { CreditProcessErrors.DeleteAndCancelConflict, "مقدار حذف یا کنسل صحیح نمی باشد" },
            { CreditProcessErrors.DebtCalculationServiceUnavailable, "به علت در دسترس نبودن سرویس محاسبه بدهی مشتری، قادر به ادامه فرآیند نمیباشید" }
        };
    }

    /// <summary>
    /// دریافت شرح خطا
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public string GetMessage(
        CreditProcessErrors error)
    {
        return _messages.TryGetValue(
            error,
            out var message)
            ? message
            : "خطای ناشناخته رخ داده است";
    }

    /// <summary>
    /// تلاش برای دریافت شرح خطا
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="errorEnum"></param>
    /// <returns></returns>
    public bool TryGetMessage(
        int code,
        out string message,
        out CreditProcessErrors? errorEnum)
    {
        if (Enum.IsDefined(typeof(CreditProcessErrors), code))
        {
            errorEnum = (CreditProcessErrors)code;
            message = GetMessage(errorEnum.Value);
            return true;
        }

        message = string.Empty;
        errorEnum = null;
        return false;
    }
}
