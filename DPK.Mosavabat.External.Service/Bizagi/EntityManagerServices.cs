using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using Microsoft.Extensions.Configuration;
using MosavabatEntityManager;
using Newtonsoft.Json;
using System.Xml;

namespace DPK.Mosavabat.External.Services.Bizagi;


public enum EntityManagerMethods
{
    SaveEntityAsync,
    GetCaseDataUsingSchemaAsync,
    GetEntitiesAsync,
}
public class EntityManagerServices : IEntityManagerServices
{

    private readonly ILoggerManager _logger;
    private readonly EntityManagerSOASoapClient _entityManagerSoaSoapClient;
    private const string BizagiEntityManagerSoaUrl = "Bizagi:EntityManagerSoaUrl";
    private const string CanNotFindEntityManagerMethodErrorMessage = "متد مذکور در بیزاجی تعریف نشده است";
    private const string ProcessErrorCodePath = "processError/errorCode";
    private const string ProcessErrorMessagePath = "processError/errorMessage";
    private const string ProcessIdPath = "process/processId";
    private const string ProcessRadNumberPath = "process/processRadNumber";
    public EntityManagerServices(ILoggerManager logger, IConfiguration configuration)
    {
        _logger = logger;
        var url = configuration[BizagiEntityManagerSoaUrl];
        _entityManagerSoaSoapClient = new EntityManagerSOASoapClient(EntityManagerSOASoapClient.EndpointConfiguration.EntityManagerSOASoap, url);
    }

    private async Task<MosavabatResponseDataModel?> ProcessCases(XmlNode casesInfo, EntityManagerMethods methodName)
    {
        var result = methodName switch
        {
            EntityManagerMethods.SaveEntityAsync => await _entityManagerSoaSoapClient.saveEntityAsync(casesInfo),
            EntityManagerMethods.GetCaseDataUsingSchemaAsync => await _entityManagerSoaSoapClient.getCaseDataUsingSchemaAsync("3751",null,null),
            EntityManagerMethods.GetEntitiesAsync => await _entityManagerSoaSoapClient.getEntitiesAsync(casesInfo),
            _ => throw new ArgumentException(CanNotFindEntityManagerMethodErrorMessage),
        };

        _logger.Trace(JsonConvert.SerializeObject(result));

        var errorCode = result.SelectSingleNode(ProcessErrorCodePath)?.InnerText;
        var errorMessage = result.SelectSingleNode(ProcessErrorMessagePath)?.InnerText;

        if (!string.IsNullOrEmpty(errorCode) && !string.IsNullOrEmpty(errorMessage))
            _logger.Error($"Code: {errorCode} | Message: {errorMessage}");

        var caseId = Convert.ToInt32(result.SelectSingleNode(ProcessIdPath)?.InnerText);
        var radNumber = Convert.ToInt32(result.SelectSingleNode(ProcessRadNumberPath)?.InnerText);

        return new MosavabatResponseDataModel(caseId, radNumber);
    }

    public async Task<MosavabatResponseDataModel?> SaveEntityAsync(XmlNode casesInfo)
    {
        return await ProcessCases(casesInfo, EntityManagerMethods.SaveEntityAsync);
    }

    public async Task<MosavabatResponseDataModel?> GetCaseDataUsingSchemaAsync(XmlNode casesInfo)
    {
        return await ProcessCases(casesInfo, EntityManagerMethods.GetCaseDataUsingSchemaAsync);
    }

    public async Task<MosavabatResponseDataModel?> GetEntitiesAsync(XmlNode casesInfo)
    {
        return await ProcessCases(casesInfo, EntityManagerMethods.GetEntitiesAsync);
    }

    public async Task<string?> GetEntitiesUsingSchemaAsync(XmlNode entitiesInfo, XmlNode schema)
    {
        var result = await _entityManagerSoaSoapClient.getEntitiesUsingSchemaAsync(entitiesInfo, schema);
        _logger.Trace(result.OuterXml);
        _logger.Trace(JsonConvert.SerializeObject(result));
        return result?.InnerXml;
    }
}