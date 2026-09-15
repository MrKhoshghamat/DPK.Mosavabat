using DPK.Mosavabat.Shared.Dictionaries.LCP;

namespace DPK.Mosavabat.Shared.Enums.LCP
{
    public static class ErrorsDictionary
    {
        public static Dictionary<Errors, string>? EDictionary;

        static ErrorsDictionary()
        {
            EDictionary = new Dictionary<Errors, string>
        {
            { Errors.Code404, "درخواست يافت نشد" },
            { Errors.Code1103, "مصوبه يافت نشد" },
            { Errors.Code1106, "مصوبه منقضی شده است" },
            { Errors.Code1113, "مصوبه سر رسید شده است" },
            { Errors.Code1122, "یک مجوز پرداخت تعیین تکلیف نشده برای این شماره مشتری وجود دارد" },
            { Errors.Code1124, "برای این مصوبه پرداخت انجام شده است" },
        };
        }
    }
}
