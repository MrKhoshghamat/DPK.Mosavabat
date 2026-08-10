using DPK.Mosavabat.Shared.Dictionaries.CreditProcesses;

namespace DPK.Mosavabat.Shared.Enums.CreditProcesses;

public static class ErrorCodes
{
    public const string UnAuthorize = "دسترسی کاربر به این سرویس غیر مجاز است";
}

public static class ErrorsDictionary
{
    public static Dictionary<Errors, string>? EDictionary;

    static ErrorsDictionary()
    {
        EDictionary = new Dictionary<Errors, string>
        {
            { Errors.Code401, "مجوز ارتباط با سرویس یافت نشد" },
            { Errors.CodeUnsuccessful, "خطا در عملیات ثبت قرارداد تکمیل اطلاعات در مصوبات" },
            { Errors.Code1100, "پارامتر نامعتبر می باشد" },
            { Errors.Code1101, "پارامتر الزامی می باشد" },
            { Errors.Code1102, "سیستم با مشکل مواجه شده است لطفا با مسئول سیستم تماس بگیرید" },
            { Errors.Code1103, "مصوبه يافت نشد" },
            { Errors.Code1104, "مبلغ قرارداد نمي تواند بزرگتر از مبلغ مصوبه باشد" },
            { Errors.Code1105, "مبلغ پيش دریافت قرارداد نمی تواند کمتر از مبلغ پیش دریافت مصوبه باشد" },
            { Errors.Code1106, "مصوبه منقضی شده است" },
            { Errors.Code1107, "تعداد قرارداد هاي مصوبه گروهی به اتمام رسیده است" },
            { Errors.Code1108, "معدل حفظ نشده است" },
            { Errors.Code1109, "مشتری چک برگشتی دارد" },
            { Errors.Code1110, "برای این مصوبه مجوز تعیین تکلیف نشده وجود دارد ابتدا برای مجوز قبلی قرارداد ثبت نمایید سپس اقدام به دریافت مجوز جدید نمایید" },
            { Errors.Code1111, "درج  مجوز تکراری مجاز نیست" },
            { Errors.Code1112, "برای مصوبه مجوز تعیین تکلیف نشده  وجود دارد امکان صدور مجوز وجود ندارد" },
            { Errors.Code1113, "مصوبه سر رسید شده است" },
            { Errors.Code1114, "مصوبه مانده آزاد ندارد" },
            { Errors.Code1115, "بیش از یک مجوز پرداخت برای تسهیلات مورد نظر وجود دارد" },
            { Errors.Code1116, "مجوز پرداخت برای تسهیلات مورد نظر یافت نشد" },
            { Errors.Code1117, "مجوز پرداخت برای تسهیلات مورد نظر تکراری است" },
            { Errors.Code1122, "یک مجوز پرداخت تعیین تکلیف نشده برای این شماره مشتری وجود دارد" },
            { Errors.Code1123, "خطا در عملیات ثبت استثنای مجوز پرداخت" },
            { Errors.Code1124, "برای این مصوبه پرداخت انجام شده است" },
            { Errors.CodeDeleteForbidden, "شماره قرارداد وجود دارد، لذا قادر به حذف مجوز پرداخت نیستید" },
            { Errors.CodeCancelForbidden,"شماره قرارداد وجود ندارد، لذا قادر به ابطال مجوز پرداخت نیستید" },
            { Errors.CodeNotFoundPaymentPermission,"شماره قرارداد یافت نشد" },
            { Errors.CodeDeleteAndCancelCanNotBothBeActive,"مقدار حذف یا کنسل صحیح نمی باشد" },
            { Errors.Code1130, "به علت در دسترس نبود سرویس محاسبه بدهی مشتری. قادر به ادامه فرآیند نمیباشید" }
        };
    }
}