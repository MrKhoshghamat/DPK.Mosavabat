using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using System.Xml;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Services.Extensions;
using DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;
using Newtonsoft.Json;
using DPK.Mosavabat.Shared.DataModels.Lcp;

namespace DPK.Mosavabat.Services;

internal class RequestFacilityService : IRequestFacilityService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;
    private readonly IRequestFacilityDatabase _requestFacilityDatabase;
    private readonly IExchangeCommercialDocDatabase _commercialDocDatabase;
    private readonly ISubContractTypeDatabase _subContractTypeDatabase;
    private readonly IFacilityGroupDatabase _facilityGroupDatabase;
    private readonly IUserDatabase _userDatabase;
    private readonly ICollateralTypeDatabase _collateralTypeDatabase;
    private readonly IWorkflowEngineServices _workflowEngine;

    public RequestFacilityService(
        ILoggerManager logger,
        IRepositoryManager repository,
        IWorkflowEngineServices workflowEngine,
        IMapper mapper,
        IUserDatabase userDatabase,
        IRequestFacilityDatabase requestFacilityDatabase,
        ICollateralTypeDatabase collateralTypeDatabase,
        IExchangeCommercialDocDatabase commercialDocDatabase,
        IFacilityGroupDatabase facilityGroupDatabase,
        ISubContractTypeDatabase subContractTypeDatabase)
    {
        _logger = logger;
        _repository = repository;
        _userDatabase = userDatabase;
        _requestFacilityDatabase = requestFacilityDatabase;
        _collateralTypeDatabase = collateralTypeDatabase;
        _commercialDocDatabase = commercialDocDatabase;
        _workflowEngine = workflowEngine;
        _subContractTypeDatabase = subContractTypeDatabase;
        _facilityGroupDatabase = facilityGroupDatabase;
    }

    public async Task<MosavabatResponseDataModel?> RegisterRequestFacilityAsync(MosavabatRequestFacilityDataModel requestFacility)
    {

        requestFacility.GUID = Guid.NewGuid().ToString();
        // جستجوی کاربر
        var userName = await _userDatabase.GetUserName(Convert.ToInt32(requestFacility.BranchCode), MosavabatClassName.LCP_RequestFacility);

        // استخراج شناسه نوع وثیقه
        foreach (var collateralDataModel in requestFacility.LCPCollaterals.Where(collateralDataModel => collateralDataModel.CollateralType != null))
        {
            var collateralTypeId = await _collateralTypeDatabase.GetId(int.Parse(collateralDataModel.CollateralType!));
            collateralDataModel.CollateralType = collateralTypeId;
        }

        // بررسی وجود شماره حساب
        var customerAccount = await _requestFacilityDatabase.FindCustomerAccountNoDataModel(new CustomerNoDataModel() { AccountNo = requestFacility.AccountNumber });

        if (customerAccount.DevProviderCustomerCount == 0) return new MosavabatResponseDataModel(default, default);

        // Add Logic to Replace Id With Code
        foreach (MosavabatSubjectFacilityDataModel subjectFacility in requestFacility.SubjectFacilities)
        {
            if (subjectFacility.RequestDocuments != null)
            {
                foreach (MosavabatContractDocumentDataModel requestDocument in subjectFacility.RequestDocuments)
                {
                    if (requestDocument != null)
                    {
                        requestDocument.DocumentType = await _commercialDocDatabase.ExtractExchangeCommercialDocId(requestDocument.DocumentType);

                    }
                }
            }
        }

        // استخراج شناسه نوع قرارداد
        foreach (var requestDocument in requestFacility.SubjectFacilities)
        {
            if (requestDocument.SubContractType != null)
            {
                requestDocument.SubContractType = await _subContractTypeDatabase.ExtractSubContractTypeId(requestDocument.SubContractType);
            }
            requestDocument.FacilityGroup = await _facilityGroupDatabase.ExtractFacilityGroupId(requestDocument.ContractType);
        }

        foreach (var item in requestFacility.LCPCollaterals)
        {
            if (item != null)
            {
                item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            }
        }

        foreach (var item in requestFacility.SubjectFacilities)
        {
            if (item != null)
            {
                item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            }
        }

        // اگر کاربر خالی بود یل تعریف نشده باشد
        if (string.IsNullOrEmpty(userName))
            return new MosavabatResponseDataModel(default, default);

        // ساخت سند
        var xmlDocument = new XmlDocument();
        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(requestFacility, userName, "LCP_ApprovalRequest");
        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        xmlDocument.LoadXml(data);

        // ساخت تسهیلات اصلی
        var response = await _workflowEngine.CreateCasesAsync(xmlDocument);

        // در صورتی که تسهیلاتی ساخته نشده باشد 
        if (response == null || response.CaseId == 0) return default;

        // ذخیره اطلاعات در دیتابیس
        await _requestFacilityDatabase.UpdateLcpRequestFacility(response.CaseId, response.RadNumber, null, null, null, requestFacility.GUID);

        return response;

    }

    public async Task<string?> GetContractFileLink(GetMorabeheContractFileLinkDataModel getMorabeheContractFileLinkDataModel)
    {
        return await _requestFacilityDatabase.GetContractFileLink(getMorabeheContractFileLinkDataModel);
    }

    public async Task<bool> UpdateContractFileLink(UpdateMorabeheContractFileLink updateMorabeheContractFileLink)
    {
        var result = await _requestFacilityDatabase.UpdateContractFileLink(updateMorabeheContractFileLink);
        return true;
    }


    public async Task<MosavabatResponseDataModel?> UpdateRequestFacility(MosavabatRequestFacilityUpdateDataModel updateRequestFacility)
    {

        var processName = "ApprovalRequest2";
        // جستجوی کاربر
        var userName = await _userDatabase.GetUserName(Convert.ToInt32(updateRequestFacility.Entity.BranchCode), MosavabatClassName.LCP_RequestFacility);
        if (string.IsNullOrEmpty(userName)) return new MosavabatResponseDataModel(default, default);

        // بررسی وجود شماره حساب
        var customerAccount = await _requestFacilityDatabase.FindCustomerAccountNoDataModel(new CustomerNoDataModel() { AccountNo = updateRequestFacility.Entity.AccountNumber });

        if (customerAccount.DevProviderCustomerCount == 0) return new MosavabatResponseDataModel(default, default);

        foreach (var collateralDataModel in updateRequestFacility.Entity.LCPCollaterals.Where(collateralDataModel => collateralDataModel.CollateralType != null))
        {
            var collateralTypeId = await _collateralTypeDatabase.GetId(int.Parse(collateralDataModel.CollateralType!));
            collateralDataModel.CollateralType = collateralTypeId;
        }


        foreach (MosavabatSubjectFacilityDataModel subjectFacility in updateRequestFacility.Entity.SubjectFacilities)
        {
            if (subjectFacility.RequestDocuments != null)
            {
                foreach (MosavabatContractDocumentDataModel requestDocument in subjectFacility.RequestDocuments)
                {
                    if (requestDocument != null)
                    {
                        requestDocument.DocumentType = await _commercialDocDatabase.ExtractExchangeCommercialDocId(requestDocument.DocumentType);
                    }
                }
            }
        }


        foreach (var requestDocument in updateRequestFacility.Entity.SubjectFacilities)
        {
            if (requestDocument.SubContractType != null)
            {
                requestDocument.SubContractType = await _subContractTypeDatabase.ExtractSubContractTypeId(requestDocument.SubContractType);
            }

            requestDocument.FacilityGroup = await _facilityGroupDatabase.ExtractFacilityGroupId(requestDocument.ContractType);
        }

        foreach (var item in updateRequestFacility.Entity.LCPCollaterals)
        {
            if (item != null)
            {
                item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            }
        }
        foreach (var item in updateRequestFacility.Entity.SubjectFacilities)
        {
            if (item != null)
            {
                item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            }
        }
        // Get Process Name From Database
        if (processName != "LCP_RequestFacility") // if ProcessName ==  ApprovalRequest2
        {
            _logger.Info("Start ApprovalRequest2");
            var approvalRequestUpdateDataModel = new MosavabatApprovalsRequestUpdateDataModel();

            approvalRequestUpdateDataModel.TaskId = updateRequestFacility.TaskId;
            approvalRequestUpdateDataModel.IdCase = updateRequestFacility.IdCase;
            approvalRequestUpdateDataModel.Entity = new MosavabatApprovalsRequest();
            approvalRequestUpdateDataModel.Entity.RequestFacility = new MosavabatRequestFacilityDataModel();
            approvalRequestUpdateDataModel.Entity.RequestFacility = updateRequestFacility.Entity;

            processName = "ApprovalRequest2";
            // We Should Have a Parent In This Node
            var standardModel = WorkFlowEngineService.GetStandardModelUpdate(approvalRequestUpdateDataModel.Entity, userName, updateRequestFacility.IdCase, updateRequestFacility.TaskId);
            var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "approvalsRequest");
            var xDocx = new XmlDocument();

            xDocx.LoadXml(data);
            // log
            _logger.Info("ApprovalRequest2: " + JsonConvert.SerializeObject(xDocx));
            var response = await _workflowEngine.PerformActivityAsync(xDocx);
            return response;
        }
        else
        {
            _logger.Info("LCP_RequestFacility");
            var standardModel = WorkFlowEngineService.GetStandardModelUpdate(updateRequestFacility.Entity, userName, updateRequestFacility.IdCase, updateRequestFacility.TaskId);
            var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, processName);
            var xDocx = new XmlDocument();

            xDocx.LoadXml(data);
            _logger.Info("LCP_RequestFacility: " + JsonConvert.SerializeObject(xDocx));
            var response = await _workflowEngine.PerformActivityAsync(xDocx);
            return response;
        }
    }

    public async Task<MosavabatResponseDataModel?> SaveRequestFacility(MosavabatRequestFacilityUpdateDataModel updateRequestFacility)
    {
        // جستجوی کاربر
        var userName = await _userDatabase.GetUserName(Convert.ToInt32(updateRequestFacility.Entity.BranchCode), MosavabatClassName.LCP_RequestFacility);
        if (string.IsNullOrEmpty(userName)) return new MosavabatResponseDataModel(default, default);

        var standardModel = WorkFlowEngineService.GenerateSaveActivityDataModel(updateRequestFacility.Entity, userName, updateRequestFacility.IdCase, updateRequestFacility.TaskId);
        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        var xDocx = new XmlDocument();

        xDocx.LoadXml(data);
        var response = await _workflowEngine.SaveActivityAsync(xDocx);

        return response;
    }

    public async Task<MosavabatResponseDataModel?> AbortCasesAsync(AbortCasesDataModel dataModel)
    {
        var standardModel = WorkFlowEngineService.GenerateAbortCasesDataModel("srv-mosavabat", dataModel.userName, dataModel.Cases.Enities);
        var data = WorkFlowEngineService.ConvertModelToXml(standardModel);
        var xDocx = new XmlDocument();

        xDocx.LoadXml(data);
        var response = await _workflowEngine.AbortCasesAsync(xDocx);

        return response;
    }
}