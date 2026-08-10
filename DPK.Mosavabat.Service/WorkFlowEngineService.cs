using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Extensions;
using DPK.Mosavabat.Shared.Templates.Bizagi.Base;
using DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Services;

public class WorkFlowEngineService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;
    private readonly IWorkflowEngineServices _workflowEngine;

    public WorkFlowEngineService(IRepositoryManager repository, IWorkflowEngineServices workflowEngine,
        ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _workflowEngine = workflowEngine;
    }

    /// <summary>
    /// مدل آماده شده برای ارسال را می گیرد
    /// در قالب استاندارد بیزاجی قرار می دهد
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">مدل آماده شده جهت ارسال</param>
    /// <param name="userName">نام کاربری دریافتی از مصوبات</param>
    /// <param name="process"></param>
    /// <returns></returns>
    public static BizAgiWSParamCreate<T> CreateStandardModelForBizagi<T>(T model, string userName, string process)
    {
        return new BizAgiWSParamCreate<T>
        {
            Domain = "srv-mosavabat",
            UserName = userName,
            Cases = new Cases<T>
            {
                Case = new Case<T>
                {
                    Process = process,
                    Entities = new Entities<T>
                    {
                        Dynamic = model
                    }
                }
            }
        };
    }

    /// <summary>
    /// مدل آماده شده برای ارسال را می گیرد
    /// در قالب استاندارد بیزاجی قرار می دهد
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">مدل آماده شده جهت ارسال</param>
    /// <param name="userName">نام کاربری دریافتی از مصوبات</param>
    /// <param name="idCase"></param>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public static BizAgiWSParamUpdate<T> GetStandardModelUpdate<T>(T model, string userName, string idCase, string taskId)
    {
        return new BizAgiWSParamUpdate<T>
        {
            Domain = "srv-mosavabat",
            UserName = userName,
            ActivityData = new MosavabatActivityDataDataModel
            {
                IdCase = idCase,
                TaskId = taskId
            },
            Entities = new Entities<T>
            {
                Dynamic = model
            }
        };
    }

    public static BizAgiWSParamUpdate<T> GenerateSaveActivityDataModel<T>(T model, string userName, string idCase, string taskId)
    {
        return new BizAgiWSParamUpdate<T>
        {
            Domain = "srv-mosavabat",
            UserName = userName,
            ActivityData = new MosavabatActivityDataDataModel
            {
                IdCase = idCase,
                TaskId = taskId
            },
            Entities = new Entities<T>
            {
                Dynamic = model
            }
        };
    }

    public static BizAgiWsParam GenerateAbortCasesDataModel(string domain, string userName, List<Case> entities)
    {
        return new BizAgiWsParam
        {
            Domain = domain,
            UserName = userName,
            Cases = new Cases()
            {
                Enities = entities
            }
        };
    }

    /// <summary>
    /// تبدیل به xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="mosavabat"></param>
    /// <returns></returns>
    public static string ConvertModelToXmlMosavabat<T>(T model, string mosavabat) where T : BizAgiWSParam
    {
        var xml = model.ConvertObjectToXml();
        return xml.Replace(nameof(Entities<int>.Dynamic), mosavabat.ToString());
    }

    public static string ConvertModelToXml<T>(T model) where T : BizAgiWsParam
    {
        return model.ConvertObjectToXml();
    }

    public static string GenerateAbortCasesXml(
    List<int> caseIds,
    string domain = "srv-mosavabat")
    {
        if (caseIds == null || caseIds.Count <= 0)
            throw new ArgumentException("CaseIds cannot be empty");

        var request = new BizAgiAbortCasesRequest
        {
            Domain = domain,
            Cases = new AbortCases
            {
                Case = caseIds.Select(id => new AbortCase
                {
                    Id = id
                }).ToList()
            }
        };

        return request.ConvertObjectToXml();
    }
}