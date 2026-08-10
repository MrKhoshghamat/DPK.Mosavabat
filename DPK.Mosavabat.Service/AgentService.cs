using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using Newtonsoft.Json;
using System.Xml;

namespace DPK.Mosavabat.Services;

internal class AgentService : IAgentService
{
    private readonly IUserDatabase _userDatabase;
    private readonly IWorkflowEngineServices _workflowEngine;
    private readonly ILoggerManager _logger;

    public AgentService(ILoggerManager logger, IWorkflowEngineServices workflowEngine, IUserDatabase userDatabase)
    {
        _logger = logger;
        _userDatabase = userDatabase;
        _workflowEngine = workflowEngine;
    }

    /// <summary>
    /// ثبت نام یک نماینده
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public async Task<MosavabatResponseDataModel?> RegisterAgentAsync(MosavabatAgentDataModel agent)
    {
        _logger.Trace(JsonConvert.SerializeObject(agent));
        var userName = await _userDatabase.GetUserName(Convert.ToInt32(agent.BranchCode), MosavabatClassName.AgentRegistry);

        if (string.IsNullOrEmpty(userName))
            return new MosavabatResponseDataModel(default, default);

        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(agent, userName, "AgentRegister");

        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "AgentRegistry");

        var xDoc = new XmlDocument();

        xDoc.LoadXml(data);

        var response = await _workflowEngine.CreateCasesAsync(xDoc);

        return response;
    }
}