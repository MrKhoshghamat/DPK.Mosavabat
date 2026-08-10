using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using Newtonsoft.Json;
using System.Xml;

namespace DPK.Mosavabat.Services;

public class ApprovalRequestService : IApprovalRequestService
{
    private readonly IUserDatabase _userDatabase;
    private readonly IApprovalRequestDatabase _approvalRequestDatabase;
    private readonly IWorkflowEngineServices _workflowEngine;
    private readonly IRequestFacilityServices _requestFacility;
    private readonly IRequestFacilityDatabase _updateRequestFacilityDatabase;
    private readonly IBranchDatabase _branchDatabase;
    private readonly ILoggerManager _logger;
    public ApprovalRequestService(
        ILoggerManager logger,
        IApprovalRequestDatabase approvalRequestDatabase,
        IWorkflowEngineServices workflowEngine,
        IUserDatabase userDatabase,
        IRequestFacilityServices requestFacilityServices,
        IRequestFacilityDatabase requestFacilityDatabase,
        IBranchDatabase branchDatabase
        )
    {
        _logger = logger;
        _approvalRequestDatabase = approvalRequestDatabase;
        _userDatabase = userDatabase;
        _workflowEngine = workflowEngine;
        _requestFacility = requestFacilityServices;
        _updateRequestFacilityDatabase = requestFacilityDatabase;
        _branchDatabase = branchDatabase;
    }

    /// <summary>
    /// برگشت وضعیت های یک درخواست
    /// </summary>
    public async Task<List<RequestFacilityStatusLevelDataModel>?> GetRequestStatus(GetRequestStatuesDataModel getRequestStatuesDataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(getRequestStatuesDataModel));
        return await _approvalRequestDatabase.GetRequestStatus(getRequestStatuesDataModel);
    }

    /// <summary>
    /// ساخت درخواست تسهیلات
    /// </summary>
    /// <param name="createApprovalRequestDataModel"></param>
    /// <returns></returns>
    public async Task<MosavabatResponseDataModel?> CreateApprovalRequest(CreateApprovalRequestDataModel createApprovalRequestDataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(createApprovalRequestDataModel));
        var response = await CreateApprovalRequest(createApprovalRequestDataModel.LcpRequestFacilityId);

        switch (response)
        {
            // اگر شناسه ای برنگشته باشد
            case { CaseId: 0 or <= 0 }:
                return response;
            // اگر پاسخی دریافت نشده باشد
            case null:
                return new MosavabatResponseDataModel(default, default);
        }

        var updateRequestFacilityRadNumber = new UpdateRequestFacilityRadNumber
        {
            CaseId = createApprovalRequestDataModel.LcpRequestFacilityIdCase,
            RadNumber = response.RadNumber,
            CaseIdApprovalRequest = response.CaseId,
        };

        // به روز رسانی شناسه قرارداد در سمت سامانه تسهیلات حقوقی
        await _requestFacility.UpdateRadNumberRequestFacility(updateRequestFacilityRadNumber);

        // به روز رسانی درخواست تسهیلات اولیه
        await _updateRequestFacilityDatabase.UpdateLcpRequestFacility(createApprovalRequestDataModel.LcpRequestFacilityIdCase, createApprovalRequestDataModel.LcpRequestFacilityRadNumber, response.CaseId, response.RadNumber, createApprovalRequestDataModel.LcpRequestFacilityId, createApprovalRequestDataModel.GuidLcpRequestFacility!);

        // کپی اطلاعات درخواست تسهیلات اولیه به درخواست تسهیلات ثانویه
        await _updateRequestFacilityDatabase.MapLcpRequestFacilityToApprovalRequest(createApprovalRequestDataModel.LcpRequestFacilityIdCase, response.CaseId, createApprovalRequestDataModel.LcpRequestFacilityId);

        // برگشت پاسخ
        return response;

    }

    private async Task<MosavabatResponseDataModel?> CreateApprovalRequest(int lcpRequestFacilityId)
    {
        _logger.Trace(JsonConvert.SerializeObject(lcpRequestFacilityId));
        var approvalsRequest = new MosavabatApprovalsRequest()
        {
            ApprovalRequestGroup = "1",
            RequestTypeDetail = "1"
        };

        // پیدا کردن کد شعبه
        var branchCode = await _branchDatabase.ExtractBranchCode(lcpRequestFacilityId);

        if (branchCode == null) return new MosavabatResponseDataModel(default, default);

        // پیدا کردن کاربر 
        var userName = await _userDatabase.GetUserName(int.Parse(branchCode), MosavabatClassName.ApprovalRequest2);

        // بررسی خالی بودن یا نبودن کاربر
        if (string.IsNullOrEmpty(userName))
            return new MosavabatResponseDataModel(default, default);

        // ساخت مدل برای ارسال به مصوبات
        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(approvalsRequest, userName, MosavabatClassName.ApprovalRequest2.ToString());

        // تبدیل فرمت
        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "approvalsRequest");

        // ساخت سند خالی
        var xDoc = new XmlDocument();

        // پر کردن سند
        xDoc.LoadXml(data);

        // ارسال سند به سرویس های Bizagi
        return await _workflowEngine.CreateCasesAsync(xDoc);

    }
}