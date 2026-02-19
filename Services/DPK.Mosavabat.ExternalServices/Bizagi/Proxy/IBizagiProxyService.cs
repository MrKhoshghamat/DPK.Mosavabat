using System.Xml;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.EntitiesDto.Bizagi;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Proxy;

public interface IBizagiProxyService
    : ITransientDependency
{
    /// <summary>
    /// ایجاد یک پرونده (Case) جدید در سیستم فرایندساز
    /// </summary>
    /// <param name="casesInfo">اطلاعات XML استاندارد برای ایجاد کیس</param>
    /// <returns>اطلاعات پرونده ایجاد شده (CaseId, RadNumber)</returns>
    Task<CreateCaseDto?> CreateCasesAsync(XmlNode? casesInfo);

    /// <summary>
    /// انجام و تایید یک فعالیت (Next Step) در فرایند
    /// </summary>
    /// <param name="casesInfo">اطلاعات XML فعالیت</param>
    Task<PerformActivityDto?> PerformActivityAsync(XmlNode? casesInfo);

    /// <summary>
    /// ذخیره وضعیت فعلی فرم بدون ارسال به مرحله بعد
    /// </summary>
    /// <param name="casesInfo">اطلاعات XML فرم</param>
    Task<SaveActivityDto?> SaveActivityAsync(XmlNode? casesInfo);

    /// <summary>
    /// ابطال یا لغو یک پرونده در جریان
    /// </summary>
    /// <param name="casesInfo">اطلاعات XML مربوط به کیس جهت ابطال</param>
    Task<AbortCaseDto?> AbortCasesAsync(XmlNode? casesInfo);

    /// <summary>
    /// ذخیره یا بروز رسانی یک موجودیت داخل بیزاجی
    /// </summary>
    /// <param name="casesInfo">XML استاندارد شامل اطلاعات موجودیت</param>
    Task<SaveEntityDto?> SaveEntityAsync(XmlNode? casesInfo);
}