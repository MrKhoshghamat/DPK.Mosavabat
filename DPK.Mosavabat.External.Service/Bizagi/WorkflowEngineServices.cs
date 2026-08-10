using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using Microsoft.Extensions.Configuration;
using MosavabatWorkflowEngine;
using Newtonsoft.Json;
using System.Threading;
using System.Xml;

namespace DPK.Mosavabat.External.Services.Bizagi
{
    public enum WorkflowMethod
    {
        CreateCasesAsync,
        PerformActivityAsync,
        SaveActivityAsync,
        AbortCasesAsync
    }
    public class WorkflowEngineServices : IWorkflowEngineServices
    {
        private readonly ILoggerManager _logger;
        private readonly WorkflowEngineSOASoapClient _workflowEngineSoaSoapClient;

        // all const
        private const string BizagiWorkflowEngineSoaUrl = "Bizagi:WorkflowEngineSoaUrl";
        private const string CanNotFindWorkflowMethodErrorMessage = "Invalid method name";
        private const string ProcessErrorCodePath = "processError/errorCode";
        private const string ProcessErrorMessagePath = "processError/errorMessage";
        private const string ProcessIdPath = "process/processId";
        private const string ProcessRadNumberPath = "process/processRadNumber";

        public WorkflowEngineServices(ILoggerManager logger, IConfiguration configuration)
        {
            _logger = logger;
            var url = configuration[BizagiWorkflowEngineSoaUrl];
            _workflowEngineSoaSoapClient = new WorkflowEngineSOASoapClient(WorkflowEngineSOASoapClient.EndpointConfiguration.WorkflowEngineSOASoap, url);
        }

        private async Task<MosavabatResponseDataModel?> ProcessCases(XmlNode casesInfo, WorkflowMethod methodName)
        {
            var result = methodName switch
            {
                WorkflowMethod.CreateCasesAsync => await _workflowEngineSoaSoapClient.createCasesAsync(casesInfo),
                WorkflowMethod.PerformActivityAsync => await _workflowEngineSoaSoapClient.performActivityAsync(casesInfo),
                WorkflowMethod.SaveActivityAsync => await _workflowEngineSoaSoapClient.saveActivityAsync(casesInfo),
                WorkflowMethod.AbortCasesAsync => await _workflowEngineSoaSoapClient.abortCasesAsync(casesInfo),
                _ => throw new ArgumentException(CanNotFindWorkflowMethodErrorMessage),
            };

            _logger.Trace(JsonConvert.SerializeObject(result));

            var errorCode = result.SelectSingleNode(ProcessErrorCodePath)?.InnerText;
            var errorMessage = result.SelectSingleNode(ProcessErrorMessagePath)?.InnerText;

            if (!string.IsNullOrEmpty(errorCode) && errorCode != "0")
            {
                _logger.Error($"Bizagi error - Code: {errorCode}, Message: {errorMessage}");
                return null;
            }

            var caseId = Convert.ToInt32(result.SelectSingleNode(ProcessIdPath)?.InnerText ?? "0");
            var radNumber = Convert.ToInt32(result.SelectSingleNode(ProcessRadNumberPath)?.InnerText ?? "0");

            return new MosavabatResponseDataModel(caseId, radNumber);
        }

        public async Task<MosavabatResponseDataModel?> CreateCasesAsync(XmlNode casesInfo)
        {
            return await ProcessCases(casesInfo, WorkflowMethod.CreateCasesAsync);
        }

        public async Task<MosavabatResponseDataModel?> PerformActivityAsync(XmlNode casesInfo)
        {
            return await ProcessCases(casesInfo, WorkflowMethod.PerformActivityAsync);
        }

        public async Task<MosavabatResponseDataModel?> SaveActivityAsync(XmlNode casesInfo)
        {
            return await ProcessCases(casesInfo, WorkflowMethod.SaveActivityAsync);
        }
        public async Task<MosavabatResponseDataModel?> AbortCasesAsync(XmlNode casesInfo)
        {
            return await ProcessCases(casesInfo, WorkflowMethod.AbortCasesAsync);
        }
    }
}
