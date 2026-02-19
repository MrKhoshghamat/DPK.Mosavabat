using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Enums.CreditProcesses;

namespace DPK.Mosavabat.BusinessServices.Abstractions.Errors;

/// <summary>
/// پرووایدر شرح خطاهای بیزینسی
/// </summary>
public interface ICreditProcessMessageProvider
    : ITransientDependency
{
    /// <summary>
    /// دریافت متن خطا بر اساس اینام
    /// </summary>
    string GetMessage(CreditProcessErrors error);

    /// <summary>
    /// تلاش برای دریافت متن خطا بر اساس کد عددی (مناسب برای خروجی دیتابیس)
    /// </summary>
    bool TryGetMessage(
        int code,
        out string message,
        out CreditProcessErrors? errorEnum);
}