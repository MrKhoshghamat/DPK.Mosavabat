using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.EntitiesDto.Bizagi;

namespace DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;

public interface IWorkflowEngineService
    : ITransientDependency
{
    /// <summary>
    /// شروع یک فرایند جدید
    /// </summary>
    Task<Result<CreateCaseDto>> CreateCaseAsync<TModel>(
        TModel model,
        string processName,
        string entityName,
        string userName);

    /// <summary>
    /// انجام فعالیت (Next)
    /// </summary>
    Task<Result<PerformActivityDto>> PerformActivityAsync<TModel>(
        TModel model);

    /// <summary>
    /// ذخیره فرم (Save)
    /// </summary>
    Task<Result<SaveActivityDto>> SaveActivityAsync<TModel>(
        TModel model);

    /// <summary>
    /// ابطال فرایند
    /// </summary>
    Task<Result<AbortCaseDto>> AbortCaseAsync<TModel>(
        TModel model);

    /// <summary>
    /// ذخیره یا بروز رسانی موجودیت داخل بیزاجی
    /// </summary>
    Task<Result<SaveEntityDto>> SaveEntityAsync<TModel>(
        TModel model);
}