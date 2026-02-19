using System.Xml;
using DPK.Foundation.UI;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.Proxy;
using DPK.Mosavabat.ExternalServices.Bizagi.Xml;
using DPK.Mosavabat.Models.EntitiesDto.Bizagi;
using Microsoft.Extensions.Logging;

namespace DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;

public class WorkflowEngineService(
    IBizagiProxyService proxyService,
    IBizagiXmlBuilder xmlBuilder,
    ILogger<WorkflowEngineService> logger)
    : IWorkflowEngineService
{
    public async Task<Result<CreateCaseDto>> CreateCaseAsync<TModel>(
        TModel model,
        string processName,
        string entityName,
        string userName = "admon")
    {
        try
        {
            var xmlPayload = xmlBuilder.BuildCreateCaseXml(
                model,
                userName,
                processName,
                entityName);

            if (xmlPayload == null)
            {
                return Result.Failure<CreateCaseDto>(
                    new Error("Bizagi.XmlError", "خطا در تولید XML برای ایجاد کیس."));
            }

            var result = await proxyService.CreateCasesAsync(xmlPayload);

            if (result == null)
            {
                return Result.Failure<CreateCaseDto>(
                    new Error("Bizagi.NoResponse", "خطا در ایجاد فرایند: پاسخی از بیزاجی دریافت نشد."));
            }

            return result;
        }
        catch (UserFriendlyException ex)
        {
            return Result.Failure<CreateCaseDto>(new Error("Bizagi.BusinessError", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "خطای سیستمی در CreateCaseAsync");
            return Result.Failure<CreateCaseDto>(new Error("Bizagi.SystemError", ex.Message));
        }
    }

    public async Task<Result<PerformActivityDto>> PerformActivityAsync<TModel>(TModel model)
    {
        try
        {
            var xmlPayload = xmlBuilder.ConvertObjectToXml(model);

            if (xmlPayload == null)
            {
                return Result.Failure<PerformActivityDto>(
                   new Error("Bizagi.XmlError", "خطا در تبدیل مدل به XML برای انجام فعالیت."));
            }

            var result = await proxyService.PerformActivityAsync(xmlPayload);

            if (result == null)
            {
                return Result.Failure<PerformActivityDto>(
                    new Error("Bizagi.NoResponse", "خطا در انجام فعالیت: پاسخی از بیزاجی دریافت نشد."));
            }

            return result;
        }
        catch (UserFriendlyException ex)
        {
            return Result.Failure<PerformActivityDto>(new Error("Bizagi.BusinessError", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "خطای سیستمی در PerformActivityAsync");
            return Result.Failure<PerformActivityDto>(new Error("Bizagi.SystemError", ex.Message));
        }
    }

    public async Task<Result<SaveActivityDto>> SaveActivityAsync<TModel>(TModel model)
    {
        try
        {
            var xmlPayload = xmlBuilder.ConvertObjectToXml(model);

            if (xmlPayload == null)
            {
                return Result.Failure<SaveActivityDto>(
                    new Error("Bizagi.XmlError", "خطا در تبدیل مدل به XML برای ذخیره فعالیت."));
            }

            var result = await proxyService.SaveActivityAsync(xmlPayload);

            if (result == null)
            {
                return Result.Failure<SaveActivityDto>(
                    new Error("Bizagi.NoResponse", "خطا در ذخیره فرم: پاسخی از بیزاجی دریافت نشد."));
            }

            return result;
        }
        catch (UserFriendlyException ex)
        {
            return Result.Failure<SaveActivityDto>(new Error("Bizagi.BusinessError", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "خطای سیستمی در SaveActivityAsync");
            return Result.Failure<SaveActivityDto>(new Error("Bizagi.SystemError", ex.Message));
        }
    }

    public async Task<Result<AbortCaseDto>> AbortCaseAsync<TModel>(TModel model)
    {
        try
        {
            var xmlPayload = xmlBuilder.ConvertObjectToXml(model);

            if (xmlPayload == null)
            {
                return Result.Failure<AbortCaseDto>(
                    new Error("Bizagi.XmlError", "خطا در تبدیل مدل به XML برای ابطال."));
            }

            var result = await proxyService.AbortCasesAsync(xmlPayload);

            if (result == null)
            {
                return Result.Failure<AbortCaseDto>(
                    new Error("Bizagi.NoResponse", "خطا در ابطال پرونده: پاسخی از بیزاجی دریافت نشد."));
            }

            return result;
        }
        catch (UserFriendlyException ex)
        {
            return Result.Failure<AbortCaseDto>(new Error("Bizagi.BusinessError", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "خطای سیستمی در AbortCaseAsync");
            return Result.Failure<AbortCaseDto>(new Error("Bizagi.SystemError", ex.Message));
        }
    }

    public async Task<Result<SaveEntityDto>> SaveEntityAsync<TModel>(TModel model)
    {
        try
        {
            var xmlPayload = xmlBuilder.BuildSaveEntityXml(model, "LCP_RequestFacility");

            if (xmlPayload == null)
            {
                return Result.Failure<SaveEntityDto>(
                    new Error("Bizagi.XmlError", "خطا در تولید XML برای ذخیره موجودیت."));
            }

            InjectBusinessKeys(xmlPayload);

            var result = await proxyService.SaveEntityAsync(xmlPayload);

            if (result == null)
            {
                return Result.Failure<SaveEntityDto>(
                    new Error("Bizagi.NoResponse", "خطا در ذخیره یا بروز رسانی موجودیت: پاسخی دریافت نشد."));
            }

            return result;
        }
        catch (UserFriendlyException ex)
        {
            return Result.Failure<SaveEntityDto>(new Error("Bizagi.BusinessError", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "خطای سیستمی در SaveEntityAsync");
            return Result.Failure<SaveEntityDto>(new Error("Bizagi.SystemError", ex.Message));
        }
    }

    /// <summary>
    /// تزریق اتریبیوت businessKey برای شناسایی رکوردهای تکراری در لیست‌ها
    /// </summary>
    private static void InjectBusinessKeys(XmlNode rootNode)
    {
        try
        {
            var lcpIdNodes = rootNode.SelectNodes("//LcpId");

            if (lcpIdNodes == null || lcpIdNodes.Count == 0) return;

            foreach (XmlNode node in lcpIdNodes)
            {
                var lcpIdValue = node.InnerText;

                // مقادیر خالی یا صفر را نادیده می‌گیریم
                if (string.IsNullOrWhiteSpace(lcpIdValue) || lcpIdValue == "0") continue;

                var parent = node.ParentNode;
                if (parent?.OwnerDocument == null) continue;

                // اگر قبلاً businessKey دارد، کاری نمی‌کنیم
                if (parent.Attributes?["businessKey"] != null) continue;

                var attr = parent.OwnerDocument.CreateAttribute("businessKey");
                attr.Value = $"LcpId='{lcpIdValue}'";
                parent.Attributes?.Append(attr);
            }
        }
        catch
        {
            // خطا در تزریق کلید نباید کل فرایند را متوقف کند، فقط لاگ یا ایگنور می‌شود
            // چون این یک متد کمکی داخلی است
        }
    }
}
