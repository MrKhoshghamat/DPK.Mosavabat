namespace DPK.Mosavabat.Common.Consts.CreditProcesses
{

    public static class CreditProcessesConst
    {
        public class ValidationHelper
        {
            public static bool CustomerNoMustGreaterThanValidator(string value)
            {
                return int.Parse(value) > 0;
            }

            public static bool NationalIdIsValidValidator(string value)
            {
                return value.Length == 10;
            }

            public static bool CheckIfValueIsValid<T>(T value) where T : IComparable
            {
                return value switch
                {
                    int intValue => intValue is >= int.MinValue or <= int.MaxValue,
                    decimal decimalValue => decimalValue is >= decimal.MinValue or <= decimal.MaxValue,
                    long longValue => longValue is >= long.MinValue or <= long.MaxValue,
                    bool boolValue => boolValue,
                    _ => throw new ArgumentException("این نوع پشتیبانی نمی شود")
                };
            }
        }
        public class Conditions
        {
            // Format
            public const string NumericFormat = @"^\d+$";

            // Min Value
            public const int ContractTypeMinValue = 0;
            public const int SubContractTypeMinValue = 0;
            public const int ContractAmountMinValue = 0;
            public const int PrepaidAmountMinValue = 0;
            public const long NationalIdMinValue = 0000000000;

            // Max Value
            public const long NationalIdMaxValue = 9999999999;

            // Max Length
            public const int ContractPrepaidAmountsSumMaxLength = 18;
            public const int CustomerNoMaxLength = 10;
            public const int MosavabeNoMaxLength = int.MaxValue;
            public const int BranchCodeMaxLength = 6;
            public const int ContractTypeMaxLength = 4;
            public const int SubContractTypeMaxLength = 5;
            public const int ContractAmountMaxLength = 18;
            public const int CustomerSharePriceMinValue = 18;

            // Min Length
            public const long C13MinValue = 1000000000000;
            public const long C13MaxValue = 9999999999999;
            public const int MosavabeNoMinLength = int.MinValue;

        }
        public class Validations
        {
            // Not Be Null
            public const string MosavabeNoMustNotBeNull = "وارد کردن شماره مصوبه الزامی می باشد";
            public const string CustomerNoMustNotBeNull = "وارد کردن شماره مشتری الزامی می باشد";
            public const string BranchCodeMustNotBeNull = "وارد کردن کد شعبه الزامی می باشد";
            public const string ContractTypeMustNotBeNull = "وارد کردن کد نوع عقد الزامی می باشد";
            public const string SubContractTypeMustNotBeNull = "وارد کردن کد زیر نوع عقد الزامی می باشد";
            public const string C13MustNotBeNull = "وارد کردن شماره قرارداد 13 رقمی الزامی می باشد";
            public const string ContractPrepaidAmountsSumMustNotBeNull = "وارد کردن جمع مبالغ قرارداد و پیش دریافت الزامی می باشد";
            public const string ConditionTitleMustNotBeNull = "وارد کردن عنوان الزامی می باشد";
            public const string NationalIdMustNotBeNull = "وارد کردن کد ملی الزامی می باشد";
            public const string LoanIdMustNotBeNull = "وارد کردن شماره تسهیلات الزامی میباشد";
            public const string ContractAmountMustNotBeNull = "وارد کردن مبلغ اصل الزامی می باشد";
            public const string CustomerSharePriceMustNotBeNull = "وارد کردن مبلغ پیش پرداخت الزامی می باشد";

            // Max Length
            public const string CustomerNoMaxLength = "شماره مشتری نمی تواند بیشتر از 10 رقم باشد";
            public const string BranchCodeMaxLength = "کد شعبه نمی تواند بیشتر از 6 رقم باشد";
            public const string ContractTypeMaxLength = "کد نوع عقد بیشتر از 4 رقم نمی تواند باشد";
            public const string SubContractTypeMaxLength = "کد زیر نوع عقد بیشتر از 5 رقم نمی تواند باشد";
            public const string ContractPrepaidAmountsSumMaxLength = "جمع مبالغ قرارداد و پیش دریافت نمی تواند بیشتر از 18 رقم باشد";
            public const string ContractAmountMaxLength = "مبلغ قرارداد نمی تواند بیشتر از 18 رقم باشد";
            public const string MosavabeNoMaxLength = "طول شماره مصوبه بیش از حد مجاز می باشد";

            // Min Length
            public const string MosavabeNoMinLength = "طول شماره مصوبه کمتر از حد مجاز می باشد";
            // Length
            public const string C13ExactlyLength = "شماره قرارداد باید 13 رقمی باشد";
            public const string NationalIdLength = "کد ملی باید 10 رقمی باشد";

            // Must Be Number
            public const string MosavabeNoMustBeNumber = "شماره مصوبه حتما باید بصورت عددی باشد";
            public const string CustomerNoMustBeNumber = "شماره مشتری حتما باید به صورت عدد باشد";
            public const string NationalIdMustBeNumber = "کد ملی حتما باید بصورت عددی باشد";
            public const string LoanIdMustBeNumber = "شماره تسهیلات حتما باید بصورت عددی باشد";

            // Must Greater Than
            public const string CustomerNoMustGreaterThan = "شماره مشتری باید  مثبت و بزرگتر از 0 باشد";

            // Can not be negative
            public const string ContractTypeCanNotBeNegative = "کد نوع عقد نمی تواند منفی باشد";
            public const string SubContractTypeCanNotBeNegative = "کد زیر نوع عقد نمی تواند منفی باشد";
            public const string CustomerSharePriceCanNotBeNegative = "مبلغ پیش پرداخت نمی تواند منفی باشد";
            public const string ContractAmountCanNotBeNegative = "مبلغ اصل قرارداد نمی تواند منفی باشد";
            public const string PrepaidAmountCanNotBeNegative = "مبلغ پیش دریافت نمی تواند منفی باشد";

            // IsNotValid
            public const string ContractTypeIsNotValid = "مقدار وارد شده برای کد نوع عقد مجاز نمی باشد";
            public const string SubContractTypeIsNotValid = "مقدار وارد شده برای کد زیر نوع عقد مجاز نمی باشد";
            public const string ContractAmountIsNotValid = "مقدار وارد شده برای مبلغ اصل قرارداد مجاز نمی باشد";
            public const string CustomerSharePriceIsNotValid = "مقدار وارد شده برای مبلغ پیش پرداخت مجاز نمی باشد";
            public const string PrepaidAmountIsNotValid = "مقدار وارد شده برای مبلغ پیش پرداخت مجاز نمی باشد";
            public const string IsDeleteIsNotValid = "مقدار پارامتر ورودی حذف به فرمت صحیح نمی باشد";
            public const string IsCancelIsNotValid = "مقدار پارامتر ورودی ابطال به فرمت صحیح نمی باشد";
            public const string BranchCodeIsNotValid = "مقدار وارد شده برای کد شعبه مجاز نمی باشد";
            public const string ContractPrepaidAmountsSumIsNotValid = "مقدار وارد شده برای کد زیر نوع عقد مجاز نمی باشد";
            public const string C13IsNotValid = "مقدار وارد شده برای شماره قرارداد 13 رقمی مجاز نمی باشد";
        }
    }
}