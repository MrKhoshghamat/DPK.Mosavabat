using DPK.Foundation.UI;
using DPK.Mosavabat.ExternalServices.Bizagi.Errors.MessageProvider;
using DPK.Mosavabat.Models.EntitiesDto.Bizagi;
using Microsoft.Extensions.Logging;
using MosavabatWorkflowEngine;
using MosavabatEntityManager;
using System.Xml;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Proxy;

public class BizagiProxyService(
    ILogger<BizagiProxyService> logger,
    IBizagiMessageProvider messageProvider,
    WorkflowEngineSOASoapClient workflowEngineClient,
    EntityManagerSOASoapClient entityManagerClient)
    : IBizagiProxyService
{
    // =================================================================================================
    // Helper Methods
    // =================================================================================================

    private void ValidateAndLogRequest(string actionName, XmlNode? requestXml)
    {
        if (requestXml == null)
            throw new ArgumentNullException(nameof(requestXml), $"XML ورودی برای سرویس {actionName} نمی‌تواند خالی باشد.");

        logger.LogInformation("Bizagi Request [{Action}]: {Xml}", actionName, requestXml.OuterXml);
    }

    private void CheckForBizagiError(XmlNode responseXml, string actionName)
    {
        var errorNode = responseXml.SelectSingleNode("processError") ??
                        responseXml.SelectSingleNode("//processError");

        if (errorNode == null) return;
        var errorCode = errorNode.SelectSingleNode("errorCode")?.InnerText;
        var errorMessage = errorNode.SelectSingleNode("errorMessage")?.InnerText;

        if (string.IsNullOrEmpty(errorCode) || errorCode == "0") return;
        logger.LogWarning("Bizagi Error [{Action}] - Code: {Code}, Msg: {Msg}", actionName, errorCode, errorMessage);
        var friendlyMessage = messageProvider.GetMessage(errorCode);
        throw new UserFriendlyException(friendlyMessage, errorMessage);
    }

    // =================================================================================================
    // Workflow Engine Methods
    // =================================================================================================

    public async Task<CreateCaseDto?> CreateCasesAsync(XmlNode? casesInfo)
    {
        const string action = nameof(CreateCasesAsync);
        ValidateAndLogRequest(action, casesInfo);

        try
        {
            var response = await workflowEngineClient.createCasesAsync(casesInfo);
            logger.LogInformation("Bizagi Response [{Action}]: {Xml}", action, response?.OuterXml);

            if (response == null) throw new UserFriendlyException("پاسخی از سرویس دریافت نشد.");
            CheckForBizagiError(response, action);

            var processNode = response.SelectSingleNode("process");
            var caseIdStr = processNode?.SelectSingleNode("processId")?.InnerText ?? "0";
            var radNumber = processNode?.SelectSingleNode("processRadNumber")?.InnerText ?? string.Empty;

            long.TryParse(caseIdStr, out var caseId);
            return new CreateCaseDto(caseId, radNumber);
        }
        catch (Exception ex) { return HandleException<CreateCaseDto>(ex, action); }
    }

    public async Task<PerformActivityDto?> PerformActivityAsync(XmlNode? activityInfo)
    {
        const string action = nameof(PerformActivityAsync);
        ValidateAndLogRequest(action, activityInfo);

        try
        {
            var response = await workflowEngineClient.performActivityAsync(activityInfo);
            logger.LogInformation("Bizagi Response [{Action}]: {Xml}", action, response?.OuterXml);

            if (response == null) throw new UserFriendlyException("پاسخی دریافت نشد.");
            CheckForBizagiError(response, action);

            var processNode = response.SelectSingleNode("process");
            long.TryParse(processNode?.SelectSingleNode("processId")?.InnerText, out var id);

            return new PerformActivityDto(id, string.Empty);
        }
        catch (Exception ex) { return HandleException<PerformActivityDto>(ex, action); }
    }

    public async Task<SaveActivityDto?> SaveActivityAsync(XmlNode? activityInfo)
    {
        const string action = nameof(SaveActivityAsync);
        ValidateAndLogRequest(action, activityInfo);

        try
        {
            var response = await workflowEngineClient.saveActivityAsync(activityInfo);
            logger.LogInformation("Bizagi Response [{Action}]: {Xml}", action, response?.OuterXml);

            if (response == null) throw new UserFriendlyException("پاسخی دریافت نشد.");
            CheckForBizagiError(response, action);

            return new SaveActivityDto(0, "Saved Successfully");
        }
        catch (Exception ex) { return HandleException<SaveActivityDto>(ex, action); }
    }

    public async Task<AbortCaseDto?> AbortCasesAsync(XmlNode? casesInfo)
    {
        const string action = nameof(AbortCasesAsync);
        ValidateAndLogRequest(action, casesInfo);

        try
        {
            var response = await workflowEngineClient.abortCasesAsync(casesInfo);
            logger.LogInformation("Bizagi Response [{Action}]: {Xml}", action, response?.OuterXml);

            if (response == null) throw new UserFriendlyException("پاسخی دریافت نشد.");
            CheckForBizagiError(response, action);

            // پاسخ Abort معمولاً شامل processId نیست یا ساختار متفاوتی دارد.
            // اگر خطایی رخ نداده باشد، یعنی موفقیت آمیز بوده.
            return new AbortCaseDto(0, "Case aborted successfully.");
        }
        catch (Exception ex) { return HandleException<AbortCaseDto>(ex, action); }
    }

    // =================================================================================================
    // Entity Manager Methods
    // =================================================================================================

    public async Task<SaveEntityDto?> SaveEntityAsync(XmlNode? entityInfo)
    {
        const string action = nameof(SaveEntityAsync);
        ValidateAndLogRequest(action, entityInfo);

        try
        {
            var response = await entityManagerClient.saveEntityAsync(entityInfo);
            logger.LogInformation("Bizagi EntityManager Response [{Action}]: {Xml}", action, response?.OuterXml);

            if (response == null) throw new UserFriendlyException("پاسخ خالی از سرویس مدیریت موجودیت.");

            var errorCodeNode = response.SelectSingleNode("//Code") ?? response.SelectSingleNode("//errorCode");
            if (errorCodeNode != null)
            {
                var code = errorCodeNode.InnerText;
                if (!string.Equals(code, "OK", StringComparison.OrdinalIgnoreCase) && code != "0")
                {
                    var msg = response.SelectSingleNode("//Message")?.InnerText ?? response.SelectSingleNode("//errorMessage")?.InnerText;
                    logger.LogWarning("EntityManager Error: {Code}, {Msg}", code, msg);
                    throw new UserFriendlyException(messageProvider.GetMessage(code), msg);
                }
            }

            long entityId = 0;

            if (long.TryParse(response.InnerText, out var simpleId))
            {
                entityId = simpleId;
            }
            else if (response.HasChildNodes)
            {
                foreach (XmlNode child in response.ChildNodes)
                {
                    if (!long.TryParse(child.InnerText, out var childId)) continue;
                    entityId = childId;
                    break;
                }
            }

            return new SaveEntityDto(entityId, "Entity Saved Successfully");
        }
        catch (Exception ex) { return HandleException<SaveEntityDto>(ex, action); }
    }

    // =================================================================================================
    // Exception Handler
    // =================================================================================================

    private T HandleException<T>(Exception ex, string actionName)
    {
        switch (ex)
        {
            case UserFriendlyException:
                throw ex;
            case System.ServiceModel.FaultException faultEx:
                logger.LogError(faultEx, "SOAP Fault in Bizagi Service [{Action}]", actionName);
                throw new UserFriendlyException("خطای پروتکل در ارتباط با سرور فرایندساز (SOAP Fault).");
            default:
                logger.LogError(ex, "Unhandled Exception in Bizagi Service [{Action}]", actionName);
                throw new UserFriendlyException("خطای سیستمی در برقراری ارتباط با فرایندساز. لطفاً با راهبر سیستم تماس بگیرید.");
        }
    }
}
