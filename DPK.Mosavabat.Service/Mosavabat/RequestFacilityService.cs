using System.Web;
using System.Xml;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Services.Extensions;
using DPK.Mosavabat.Services.Helper;
using DPK.Mosavabat.Shared.DataModels.ForLcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Newtonsoft.Json;
using DPK.Mosavabat.Shared.Utilities;
using System.Text.RegularExpressions;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using UpdateRequestFacilityDto = DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract.UpdateRequestFacilityDto;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using AutoMapper;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.External.Services.Contracts.Lms;

namespace DPK.Mosavabat.Services.Mosavabat;

public class RequestFacilityService : IRequestFacilityService
{
    private readonly ILoggerManager _logger;
    private readonly IRequestFacilityDatabaseService _requestFacilityDatabaseService;
    private readonly IUserDatabaseService _userDatabaseService;
    private readonly IExchangeCommercialDocDatabaseService _exchangeCommercialDocDatabaseService;
    private readonly ISubContractTypeDatabaseService _subContractTypeDatabaseService;
    private readonly IFacilityGroupDatabaseService _facilityGroupDatabaseService;
    private readonly ICollateralTypeDatabaseService _collateralTypeDatabaseService;
    private readonly IWorkflowEngineServices _workflowEngineServices;
    private readonly IUserDatabase _userDatabase;
    private readonly ICollateralTypeDatabase _collateralTypeDatabase;
    private readonly IExchangeCommercialDocDatabase _commercialDocDatabase;
    private readonly ISubContractTypeDatabase _subContractTypeDatabase;
    private readonly IFacilityGroupDatabase _facilityGroupDatabase;
    private readonly IWorkflowEngineServices _workflowEngine;
    private readonly IRequestFacilityDatabase _requestFacilityDatabase;
    private readonly IEntityManagerServices _entityManagerServices;
    private readonly ILoanBillProcessingService _loanBillProcessingService;
    private readonly IContractTransactionService _contractTransactionService;
    private readonly ICreditProcessService _creditProcessService;
    private readonly ICreditProcessesDatabaseService _creditProcessesDatabaseService;
    private readonly IContractGroupDatabaseService _contractGroupDatabaseService;
    private readonly ILcpServiceProxy _proxy;
    private readonly IMapper _mapper;

    public RequestFacilityService(
        ILoggerManager logger,
        IWorkflowEngineServices workflowEngine,
        IUserDatabase userDatabase,
        IRequestFacilityDatabaseService requestFacilityDatabaseService,
        IUserDatabaseService userDatabaseService,
        IExchangeCommercialDocDatabaseService exchangeCommercialDocDatabaseService,
        ISubContractTypeDatabaseService subContractTypeDatabaseService,
        IFacilityGroupDatabaseService facilityGroupDatabaseService,
        ICollateralTypeDatabaseService collateralTypeDatabaseService,
        ICollateralTypeDatabase collateralTypeDatabase,
        IExchangeCommercialDocDatabase commercialDocDatabase,
        IRequestFacilityDatabase requestFacilityDatabase,
        IFacilityGroupDatabase facilityGroupDatabase,
        ISubContractTypeDatabase subContractTypeDatabase,
        IEntityManagerServices entityManagerServices,
        IWorkflowEngineServices workflowEngineServices,
        ILoanBillProcessingService loanBillProcessingService,
        IContractTransactionService contractTransactionService,
        ICreditProcessService creditProcessService,
        ICreditProcessesDatabaseService creditProcessesDatabaseService,
        IContractGroupDatabaseService contractGroupDatabaseService,
        IMapper mapper,
        ILcpServiceProxy proxy
        )
    {
        _logger = logger;
        _userDatabase = userDatabase;
        _requestFacilityDatabaseService = requestFacilityDatabaseService;
        _collateralTypeDatabase = collateralTypeDatabase;
        _commercialDocDatabase = commercialDocDatabase;
        _userDatabaseService = userDatabaseService;
        _exchangeCommercialDocDatabaseService = exchangeCommercialDocDatabaseService;
        _subContractTypeDatabaseService = subContractTypeDatabaseService;
        _facilityGroupDatabaseService = facilityGroupDatabaseService;
        _collateralTypeDatabaseService = collateralTypeDatabaseService;
        _workflowEngineServices = workflowEngineServices;
        _subContractTypeDatabase = subContractTypeDatabase;
        _facilityGroupDatabase = facilityGroupDatabase;
        _workflowEngine = workflowEngine;
        _requestFacilityDatabase = requestFacilityDatabase;
        _entityManagerServices = entityManagerServices;
         _loanBillProcessingService = loanBillProcessingService;
        _contractTransactionService = contractTransactionService;
        _creditProcessService = creditProcessService;
        _creditProcessesDatabaseService = creditProcessesDatabaseService;
        _contractGroupDatabaseService  = contractGroupDatabaseService;
        _proxy = proxy;
        _mapper = mapper;
    }

    public async Task<RegisterRequestFacilityDto?> RegisterRequestFacilityAsync(RegisterRequestFacilityVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        viewModel.MosavabatRequestFacilityDataModel.GUID = Guid.NewGuid().ToString();

        var getUserNameResult = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = Convert.ToInt32(viewModel.MosavabatRequestFacilityDataModel.BranchCode),
            MosavabatClassName = MosavabatClassName.LCP_RequestFacility
        }, cancellationToken);

        foreach (var collateralDataModel in viewModel.MosavabatRequestFacilityDataModel.LCPCollaterals.Where(collateralDataModel => collateralDataModel.CollateralType != null))
        {
            var collateralType = await _collateralTypeDatabaseService.GetCollateralTypeId(new GetCollateralTypeIdVm()
            {
                Code = collateralDataModel.CollateralType
            }, cancellationToken);

            collateralDataModel.CollateralType = collateralType?.CollateralTypeId;
        }

        foreach (var requestDocument in viewModel.MosavabatRequestFacilityDataModel.SubjectFacilities.SelectMany(subjectFacility => subjectFacility.RequestDocuments))
        {
            requestDocument.DocumentType = _exchangeCommercialDocDatabaseService.ExtractExchangeCommercialDocId(new ExtractExchangeCommercialDocIdVm()
            {
                DocumentCode = requestDocument.DocumentType
            }, cancellationToken).Result?.DocumentId;
        }

        foreach (var requestDocument in viewModel.MosavabatRequestFacilityDataModel.SubjectFacilities)
        {
            if (requestDocument.SubContractType != null)
            {
                requestDocument.SubContractType = _subContractTypeDatabaseService.ExtractSubContractTypeId(new ExtractSubContractTypeIdVm()
                {
                    ContractTemplateCode = requestDocument.SubContractType
                }, cancellationToken).Result?.SubContractId;
            }
            requestDocument.FacilityGroup = _facilityGroupDatabaseService.ExtractFacilityGroupId(new ExtractFacilityGroupIdVm()
            {
                Code = requestDocument.ContractType
            }, cancellationToken).Result?.FacilityGroupId;
        }

        foreach (var item in viewModel.MosavabatRequestFacilityDataModel.LCPCollaterals)
        {
            item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
        }

        foreach (var item in viewModel.MosavabatRequestFacilityDataModel.SubjectFacilities)
        {
            item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
        }

        if (string.IsNullOrEmpty(getUserNameResult.Username))
            return new RegisterRequestFacilityDto()
            { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };

        var xmlDocument = new XmlDocument();
        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(viewModel.MosavabatRequestFacilityDataModel, getUserNameResult.Username, "LCP_ApprovalRequest");
        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        xmlDocument.LoadXml(data);

        var response = await _workflowEngineServices.CreateCasesAsync(xmlDocument);

        if (response == null || response.CaseId == 0) return default;

        await _requestFacilityDatabaseService.UpdateLcpRequestFacility(new UpdateLcpRequestFacilityVm()
        {
            IdCase = response.CaseId,
            InitialRadNumber = response.RadNumber,
            IdCaseApprovalRequest = null,
            RadNumberApprovalRequest = null,
            LcpRequestFacilityId = null,
            GuidLcpRequestFacility = viewModel.MosavabatRequestFacilityDataModel.GUID
        }, cancellationToken);

        return new RegisterRequestFacilityDto() { MosavabatResponseDataModel = response };
    }

    public async Task<GetContractFileLinkDto?> GetContractFileLink(GetContractFileLinkVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _requestFacilityDatabaseService.GetContractFileLink(viewModel, cancellationToken);
    }

    public async Task<UpdateContractFileLinkDto> UpdateContractFileLink(UpdateContractFileLinkVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _requestFacilityDatabaseService.UpdateContractFileLink(viewModel, cancellationToken);
    }

    public async Task<UpdateRequestFacilityDto?> UpdateRequestFacility(UpdateRequestFacilityVm viewModel, CancellationToken cancellationToken = default)
    {
        var processName = "ApprovalRequest2";

        var getUserName = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = int.Parse(viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.BranchCode),
            MosavabatClassName = MosavabatClassName.LCP_RequestFacility
        }, cancellationToken);

        if (string.IsNullOrEmpty(getUserName.Username))
            return new UpdateRequestFacilityDto()
            { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };

        foreach (var collateralDataModel in viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.LCPCollaterals.Where(collateralDataModel => collateralDataModel.CollateralType != null))
        {
            var collateralType = await _collateralTypeDatabaseService.GetCollateralTypeId(new GetCollateralTypeIdVm()
            {
                Code = collateralDataModel.CollateralType
            }, cancellationToken);

            collateralDataModel.CollateralType = collateralType?.CollateralTypeId;
        }

        foreach (var requestDocument in viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.SubjectFacilities.SelectMany(subjectFacility => subjectFacility.RequestDocuments))
        {
            requestDocument.DocumentType = _exchangeCommercialDocDatabaseService.ExtractExchangeCommercialDocId(new ExtractExchangeCommercialDocIdVm()
            {
                DocumentCode = requestDocument.DocumentType
            }, cancellationToken).Result?.DocumentId;
        }

        foreach (var requestDocument in viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.SubjectFacilities)
        {
            if (requestDocument.SubContractType != null)
            {
                requestDocument.SubContractType = _subContractTypeDatabaseService.ExtractSubContractTypeId(new ExtractSubContractTypeIdVm()
                {
                    ContractTemplateCode = requestDocument.SubContractType
                }, cancellationToken).Result?.SubContractId;
            }
            requestDocument.FacilityGroup = _facilityGroupDatabaseService.ExtractFacilityGroupId(new ExtractFacilityGroupIdVm()
            {
                Code = requestDocument.ContractType
            }, cancellationToken).Result?.FacilityGroupId;
        }

        foreach (var item in viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.LCPCollaterals)
        {
            item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
        }

        foreach (var item in viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.SubjectFacilities)
        {
            item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
        }

        if (processName != "LCP_RequestFacility")
        {
            _logger.Info("Start ApprovalRequest2");

            var approvalRequestUpdateDataModel = new MosavabatApprovalsRequestUpdateDataModel
            {
                TaskId = viewModel.MosavabatRequestFacilityUpdateDataModel.TaskId,
                IdCase = viewModel.MosavabatRequestFacilityUpdateDataModel.IdCase,
                Entity = new MosavabatApprovalsRequest
                {
                    RequestFacility = new MosavabatRequestFacilityDataModel()
                }
            };

            approvalRequestUpdateDataModel.Entity.RequestFacility = viewModel.MosavabatRequestFacilityUpdateDataModel.Entity;

            processName = "ApprovalRequest2";

            var standardModel = WorkFlowEngineService.GetStandardModelUpdate(approvalRequestUpdateDataModel.Entity, getUserName.Username, viewModel.MosavabatRequestFacilityUpdateDataModel.IdCase, viewModel.MosavabatRequestFacilityUpdateDataModel.TaskId);
            var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "approvalsRequest");
            var xDocx = new XmlDocument();

            xDocx.LoadXml(data);

            _logger.Info("ApprovalRequest2: " + JsonConvert.SerializeObject(xDocx));
            var response = await _workflowEngineServices.PerformActivityAsync(xDocx);
            return new UpdateRequestFacilityDto() { MosavabatResponseDataModel = response };
        }
        else
        {
            _logger.Info("LCP_RequestFacility");
            var standardModel = WorkFlowEngineService.GetStandardModelUpdate(viewModel.MosavabatRequestFacilityUpdateDataModel.Entity, getUserName.Username, viewModel.MosavabatRequestFacilityUpdateDataModel.IdCase, viewModel.MosavabatRequestFacilityUpdateDataModel.TaskId);
            var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, processName);
            var xDocx = new XmlDocument();

            xDocx.LoadXml(data);
            _logger.Info("LCP_RequestFacility: " + JsonConvert.SerializeObject(xDocx));
            var response = await _workflowEngineServices.PerformActivityAsync(xDocx);
            return new UpdateRequestFacilityDto() { MosavabatResponseDataModel = response };
        }
    }

    public async Task<SaveRequestFacilityDto?> SaveRequestFacility(SaveRequestFacilityVm viewModel, CancellationToken cancellationToken = default)
    {
        var getUserName = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = int.Parse(viewModel.MosavabatRequestFacilityUpdateDataModel.Entity.BranchCode),
            MosavabatClassName = MosavabatClassName.LCP_RequestFacility
        }, cancellationToken);

        if (string.IsNullOrEmpty(getUserName.Username))
            return new SaveRequestFacilityDto()
            { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };

        var standardModel = WorkFlowEngineService.GenerateSaveActivityDataModel(viewModel.MosavabatRequestFacilityUpdateDataModel.Entity, getUserName.Username, viewModel.MosavabatRequestFacilityUpdateDataModel.IdCase, viewModel.MosavabatRequestFacilityUpdateDataModel.TaskId);
        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        var xDocx = new XmlDocument();

        xDocx.LoadXml(data);
        var response = await _workflowEngineServices.SaveActivityAsync(xDocx);

        return new SaveRequestFacilityDto() { MosavabatResponseDataModel = response };
    }

    public async Task<AbortCasesDto> AbortCasesAsync(AbortCasesVm viewModel, CancellationToken cancellationToken = default)
    {
        var standardModel = WorkFlowEngineService.GenerateAbortCasesDataModel("srv-mosavabat", viewModel.userName, viewModel.Cases.Enities);
        var data = WorkFlowEngineService.ConvertModelToXml(standardModel);
        var xDocx = new XmlDocument();

        xDocx.LoadXml(data);
        var response = await _workflowEngineServices.AbortCasesAsync(xDocx);

        return new AbortCasesDto() { MosavabatResponseDataModel = response };
    }

    public async Task<MosavabatResponseDataModel?> RegisterRequestFacilityAsyncV2(RequestFacilityUpdateDataModel requestFacility)
    {
        requestFacility.GUID = Guid.NewGuid().ToString();
        // جستجوی کاربر
        var userName = await _userDatabase.GetUserName(Convert.ToInt32(requestFacility.BranchCode), MosavabatClassName.LCP_RequestFacility);
        requestFacility.LcpApprovelStatuse = "5";
        // استخراج شناسه نوع وثیقه
        foreach (var collateralDataModel in requestFacility.Collaterals.Collateral.Where(collateralDataModel => collateralDataModel.CollateralType != null))
        {
            var collateralTypeId = await _collateralTypeDatabase.GetId(int.Parse(collateralDataModel.CollateralType!));
            collateralDataModel.CollateralType = collateralTypeId;
        }

        // Add Logic to Replace Id With Code
        foreach (var subjectFacility in requestFacility.SubjectFacilities.SubjectFacility)
        {
            if (subjectFacility.RequestDocument != null)
            {
                foreach (var requestDocument in subjectFacility.RequestDocument.RequestDocument)
                {
                    if (requestDocument != null)
                    {
                        requestDocument.DocumentType = await _commercialDocDatabase.ExtractExchangeCommercialDocId(requestDocument.DocumentType);

                    }
                }
            }
        }


        // استخراج شناسه نوع قرارداد
        foreach (var requestDocument in requestFacility.SubjectFacilities.SubjectFacility)
        {
            requestDocument.FacilityGroup = await _facilityGroupDatabase.ExtractFacilityGroupId(requestDocument.ContractType);

            if (requestDocument.SubContractType != null)
            {
                requestDocument.SubContractType = await _subContractTypeDatabase.ExtractSubContractTypeId(requestDocument.SubContractType);
            }

            if (requestDocument.ContractGroup != null)
            {
                requestDocument.ContractGroup = _contractGroupDatabaseService.ExtractContractGroupId(new ExtractContractGroupIdVm()
                {
                    ContractGroupCode = requestDocument.ContractGroup
                }).Result?.ContractGroupId;

                if (requestDocument.ContractGroup == "11" || requestDocument.ContractGroup == "12")
                {
                    requestDocument.FacilityGroup = "315";
                }
            }
        }

        foreach (var item in requestFacility.Collaterals.Collateral)
        {
            if (item != null)
            {
                item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
            }
        }

        foreach (var item in requestFacility.SubjectFacilities.SubjectFacility)
        {
            if (item != null)
            {
                item.Amount = item.Amount.ToThousandSeparetedWithOutDecimals();
                item.Description = Regex.Replace(item.Description, "&amp;nbsp;", " ");
                item.SubjectContract = Regex.Replace(item.SubjectContract, "&amp;nbsp;", " ");
            }
        }

        ////Fill Rank And Score
        //if (requestFacility.MosavabatLegalRanking != null)
        //{
        //    var lcpCustomerRanksDataModel = new LcpCustomerRanksDataModel();
        //    var datamodel = new LcpCustomerRankDataModel();

        //    if (requestFacility.MosavabatLegalRanking.LocalRanking != null)
        //    {
        //        datamodel.CreditRank = requestFacility.MosavabatLegalRanking.LocalRanking.Symbol != null ? requestFacility.MosavabatLegalRanking.LocalRanking.Symbol.ToString() : null;
        //        datamodel.CreditScore = requestFacility.MosavabatLegalRanking.LocalRanking.Score != null ? Convert.ToDecimal(requestFacility.MosavabatLegalRanking.LocalRanking.Score) : null;
        //    }

        //    if (requestFacility.MosavabatLegalRanking.IranianRanking != null)
        //    {
        //        datamodel.IranianRank = requestFacility.MosavabatLegalRanking.IranianRanking.Symbol != null ? requestFacility.MosavabatLegalRanking.IranianRanking.Symbol.ToString() : null;
        //        datamodel.IranianScore = requestFacility.MosavabatLegalRanking.IranianRanking.Score != null ? Convert.ToDecimal(requestFacility.MosavabatLegalRanking.IranianRanking.Score) : null;
        //    }

        //    if (requestFacility.MosavabatLegalRanking.ExternalRanking != null)
        //    {
        //        datamodel.ExternalRankAssignDate = requestFacility.MosavabatLegalRanking.ExternalRanking.RateAssignDate != null ? Convert.ToDateTime(requestFacility.MosavabatLegalRanking.ExternalRanking.RateAssignDate) : null;
        //        datamodel.ExternalRankExpireDate = requestFacility.MosavabatLegalRanking.ExternalRanking.RateExpireDate != null ? Convert.ToDateTime(requestFacility.MosavabatLegalRanking.ExternalRanking.RateExpireDate) : null;
        //        datamodel.ExternalShortTermRank = requestFacility.MosavabatLegalRanking.ExternalRanking.ShortTermRateSymbol != null ? requestFacility.MosavabatLegalRanking.ExternalRanking.ShortTermRateSymbol.ToString() : null;
        //        datamodel.ExternalScore = requestFacility.MosavabatLegalRanking.ExternalRanking.Score != null ? Convert.ToDecimal(requestFacility.MosavabatLegalRanking.ExternalRanking.Score) : null;
        //        datamodel.ExternalRankFile = requestFacility.MosavabatLegalRanking.ExternalRanking.FileUrl != null ? requestFacility.MosavabatLegalRanking.ExternalRanking.FileUrl : null;
        //        datamodel.ExternalLongTermRank = requestFacility.MosavabatLegalRanking.ExternalRanking.LongTermRateSymbol != null ? requestFacility.MosavabatLegalRanking.ExternalRanking.LongTermRateSymbol.ToString() : null;
        //    }

        //    lcpCustomerRanksDataModel.LcpCustomerRank.Add(datamodel);
        //    requestFacility.LcpCustomerRanks = lcpCustomerRanksDataModel;

        //}

        ////Fill Rank And Score In Guarantors
        //if (requestFacility.Guarantors != null)
        //{
        //    foreach (var guarantor in requestFacility.Guarantors.Guarantor)
        //    {
        //        var guarantorRankDataModel = new LcpGuarantorRanksDataModel();
        //        var dataModel = new LcpGuarantorRankDataModel();

        //        if (guarantor.IranianRanking != null)
        //        {
        //            dataModel.IranianRank = guarantor.IranianRanking.Symbol != null ? guarantor.IranianRanking.Symbol.ToString() : null;
        //            dataModel.IranianScore = guarantor.IranianRanking.Score != null ? Convert.ToDecimal(guarantor.IranianRanking.Score) : null;
        //        }

        //        if (guarantor.MosavabatLegalRanking != null)
        //        {
        //            if (guarantor.MosavabatLegalRanking.LocalRanking != null)
        //            {
        //                dataModel.CreditRank = guarantor.MosavabatLegalRanking.LocalRanking.Symbol != null ? guarantor.MosavabatLegalRanking.LocalRanking.Symbol.ToString() : null;
        //                dataModel.CreditScore = guarantor.MosavabatLegalRanking.LocalRanking.Score != null ? Convert.ToDecimal(guarantor.MosavabatLegalRanking.LocalRanking.Score) : null;
        //            }

        //            if (guarantor.MosavabatLegalRanking.IranianRanking != null)
        //            {
        //                dataModel.IranianRank = guarantor.MosavabatLegalRanking.IranianRanking.Symbol != null ? guarantor.MosavabatLegalRanking.IranianRanking.Symbol.ToString() : null;
        //                dataModel.IranianScore = guarantor.MosavabatLegalRanking.IranianRanking.Score != null ? Convert.ToDecimal(guarantor.MosavabatLegalRanking.IranianRanking.Score) : null;
        //            }

        //            if (guarantor.MosavabatLegalRanking.ExternalRanking != null)
        //            {
        //                dataModel.ExternalRankAssignDate = guarantor.MosavabatLegalRanking.ExternalRanking.RateAssignDate != null ? Convert.ToDateTime(guarantor.MosavabatLegalRanking.ExternalRanking.RateAssignDate) : null;
        //                dataModel.ExternalRankExpireDate = guarantor.MosavabatLegalRanking.ExternalRanking.RateExpireDate != null ? Convert.ToDateTime(guarantor.MosavabatLegalRanking.ExternalRanking.RateExpireDate) : null;
        //                dataModel.ExternalShortTermRank = guarantor.MosavabatLegalRanking.ExternalRanking.ShortTermRateSymbol != null ? guarantor.MosavabatLegalRanking.ExternalRanking.ToString() : null;
        //                dataModel.ExternalScore = guarantor.MosavabatLegalRanking.ExternalRanking.Score != null ? Convert.ToDecimal(guarantor.MosavabatLegalRanking.ExternalRanking.Score) : null;
        //                dataModel.ExternalRankFile = guarantor.MosavabatLegalRanking.ExternalRanking.FileUrl != null ? guarantor.MosavabatLegalRanking.ExternalRanking.FileUrl : null;
        //                dataModel.ExternalLongTermRank = guarantor.MosavabatLegalRanking.ExternalRanking.LongTermRateSymbol != null ? guarantor.MosavabatLegalRanking.ExternalRanking.LongTermRateSymbol.ToString() : null;
        //            }
        //        }

        //        guarantorRankDataModel.LcpGuarantorRank.Add(dataModel);
        //        guarantor.GuarantorRanks = guarantorRankDataModel;
        //    }
        //}


        // اگر کاربر خالی بود یل تعریف نشده باشد
        if (string.IsNullOrEmpty(userName))
            return new MosavabatResponseDataModel(default, default);

        // ساخت سند
        var xmlDocument = new XmlDocument();
        var standardModel = BizagiHelpers.CreateStandardModelForBizagi(requestFacility, userName, "LCP_ApprovalRequest");
        var data = BizagiHelpers.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        xmlDocument.LoadXml(data);

        // ساخت تسهیلات اصلی
        var response = await _workflowEngine.CreateCasesAsync(xmlDocument);

        // در صورتی که تسهیلاتی ساخته نشده باشد 
        if (response == null || response.CaseId == 0) return default;

        // ذخیره اطلاعات در دیتابیس
        await _requestFacilityDatabase.UpdateLcpRequestFacility(response.CaseId, response.RadNumber, null, null, null, requestFacility.GUID);

        return response;
    }

    public async Task<MosavabatResponseDataModel?> NewUpdateRequestFacilityV2(RequestFacilityUpdateDataModel requestFacility, CancellationToken cancellationToken = default)
    {
        var updateReqFacilityStatusDataModel = new UpdateReqFacilityStatusDataModel
        {
            CaseId = requestFacility.CaseId
        };
        var approvalRequestId = await _requestFacilityDatabaseService.GetApprovalRequestId(updateReqFacilityStatusDataModel, cancellationToken);

        if (requestFacility.SummarizeDifference != null)
        {
            var raw = requestFacility.SummarizeDifference;
            var diff = JsonHelper.ParseJsonString(raw);
            var summarizeDifference = JsonHelper.ExtractFlatFieldChanges(diff);

            requestFacility.LcpHistoryLogs = new Shared.DataModels.Lcp.LcpHistoryLogsDataModel
            {
                LcpHistoryLogs = summarizeDifference
                .Select(sd => new Shared.DataModels.Lcp.LcpHistoryLogDataModel
                {
                    Title = sd.Path,
                    NewValue = sd.NewValue,
                    OldValue = sd.OldValue,
                    ApprovalRequestId = (!string.IsNullOrWhiteSpace(approvalRequestId)) ? approvalRequestId : null
                }).ToList()
            };
        }

        var xmlDocument = new XmlDocument();
        _logger.Trace(JsonConvert.SerializeObject(requestFacility));
        _logger.Trace(JsonConvert.SerializeObject(updateReqFacilityStatusDataModel));
        var result = await _requestFacilityDatabaseService.UpdateReqFacilityStatus(updateReqFacilityStatusDataModel, cancellationToken);
        _logger.Trace(JsonConvert.SerializeObject(result));
        requestFacility.LcpApprovelStatuse = "5";
        var standardModel = BizagiEntityManagerHelpers.GenerateSaveActivityDataModel(requestFacility);
        var data = BizagiEntityManagerHelpers.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        xmlDocument.LoadXml(data);

        var lcpIdNodeList = xmlDocument.GetElementsByTagName("LcpId");
        foreach (XmlNode lcpIdNode in lcpIdNodeList)
        {
            var businessKeyAttribute = xmlDocument.CreateAttribute("businessKey");
            businessKeyAttribute.Value = $"LcpId='{lcpIdNode.InnerText}'";
            lcpIdNode.ParentNode?.Attributes?.Append(businessKeyAttribute);
        }

        var lcpContractDocuments = xmlDocument.GetElementsByTagName("LCP_ContractDocument");
        var response = await _entityManagerServices.SaveEntityAsync(xmlDocument);

        return response;
    }

    public async Task<CheckCustomerAccountExistsDto> CheckCustomerAccountExistsAsync(CheckCustomerAccountExistsDm dataModel, CancellationToken cancellationToken = default)
    {
        return await _requestFacilityDatabaseService.CheckCustomerAccountExistsAsync(dataModel, cancellationToken);
    }

    public async Task<string?> GetEntitiesUsingSchemaAsyncV2(FilterRequestFacilityDataModel dataModel)
    {
        // Set Entity Name
        const string entityName = "LCP_RequestFacility";
        var standardModel = BizagiEntityManagerHelpers.CreateFilterModel(entityName, dataModel.CaseId);
        var data = BizagiEntityManagerHelpers.ConvertModelToXmlMosavabat(standardModel, "LCP_RequestFacility");
        var entitiesInfo = new XmlDocument();
        entitiesInfo.LoadXml(HttpUtility.HtmlDecode(data));
        //var entitiesInfo = BizagiEntityManagerHelpers.ReadXmlFromFile("Template\\search.xml");
        var schema = BizagiEntityManagerHelpers.ReadXmlFromFile("Template\\requestFacility2.xml");
        // Create base search model for pass to get GetEntitiesAsync
        var result = await _entityManagerServices.GetEntitiesUsingSchemaAsync(entitiesInfo, schema);
        return result;
    }

    public async Task<UpdateLcpRequestRadNumberDto> UpdateLcpRequestRadNumber(UpdateLcpRequestRadNumberDataModel dataModel, CancellationToken cancellationToken)
    {
        return await _requestFacilityDatabaseService.UpdateLcpRequestRadNumber(dataModel, cancellationToken);
    }
    public async Task<UpdateLcpRequestRadNumberDto> InsertReqFacilityStatus(InsertReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken)
    {
        return await _requestFacilityDatabaseService.InsertReqFacilityStatus(dataModel, cancellationToken);
    }

    public async Task<DPK.Mosavabat.Shared.EntitiesDto.Lcp.RemoveRequestFromCartableDto> RemoveRequestFromCartable(AbortCasesVm viewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var standardModel = WorkFlowEngineService.GenerateAbortCasesDataModel("srv-mosavabat", viewModel.userName, viewModel.Cases.Enities);
            var data = WorkFlowEngineService.ConvertModelToXml(standardModel);
            var xDocx = new XmlDocument();

            xDocx.LoadXml(data);

            var response = await _workflowEngineServices.AbortCasesAsync(xDocx);

            if (response == null)
            {
                return new RemoveRequestFromCartableDto
                {
                    IsRemoved = false,
                    ErrorMessage = "امکان حذف پرونده از کارتابل وجود ندارد."
                };
            }

            return new RemoveRequestFromCartableDto
            {
                IsRemoved = true
            };
        }
        catch (XmlException)
        {
            _logger.Error(
                $"Invalid AbortCases XML. CaseId={viewModel.Cases.Enities[0].radNumber}");

            return new RemoveRequestFromCartableDto
            {
                IsRemoved = false,
                ErrorMessage = "خطا در ساختار داده ارسالی به موتور فرایند"
            };
        }
        catch (OperationCanceledException)
        {
            _logger.Warn(
                $"Abort case operation cancelled. CaseId={viewModel.Cases.Enities[0].radNumber}");

            return new RemoveRequestFromCartableDto
            {
                IsRemoved = false,
                ErrorMessage = "عملیات لغو شد"
            };
        }
        catch (Exception)
        {
            _logger.Error(
                $"Unexpected error while aborting Bizagi case. CaseId={viewModel.Cases.Enities[0].radNumber}");

            return new RemoveRequestFromCartableDto
            {
                IsRemoved = false,
                ErrorMessage = "خطای سیستمی در حذف پرونده از کارتابل"
            };
        }
    }

    public async Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _requestFacilityDatabaseService.GetRadNumberByCaseId(viewModel, cancellationToken);
    }

    public async Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _requestFacilityDatabaseService.GetUserNameByCaseId(viewModel, cancellationToken);
    }

    public async Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _requestFacilityDatabaseService.GetUserIdByUserName(viewModel, cancellationToken);
    }

    public async Task LcpRequestFacilityAbortCase(LcpRequestFacilityAbortCaseVm viewModel, CancellationToken cancellationToken)
    {
        await _requestFacilityDatabaseService.LcpRequestFacilityAbortCase(viewModel, cancellationToken);
    }

    public async Task<List<GetCustomerDebtDetailsDto>> GetCustomerDebtDetails(GetCustomerDebtDetailsDataModel dataModel, CancellationToken cancellationToken = default)
    {
        // Step 1: Initiate background processing
        await _loanBillProcessingService.InitiateAllLoanBillProcessing(dataModel.CustomerNumber , cancellationToken);

        // Step 2: Wait briefly for processing to complete (max 20 seconds)
        await _loanBillProcessingService
            .WaitForProcessingToCompleteOrTimeout(
                dataModel.CustomerNumber,
                TimeSpan.FromSeconds(20),
                cancellationToken);

        // Step 3: Get results (from cache or DB)
        var cacheKey = $"ProcessedAllContractsByCustomerNo_{dataModel.CustomerNumber}";
        var cached = _loanBillProcessingService
            .TryGetCustomerDebtDetailsFromCacheByCustomerNo(cacheKey, cancellationToken);


        var contracts = cached?? await _loanBillProcessingService
            .GetProcessedAllContractsByCustomerNo(dataModel.CustomerNumber, cancellationToken);

        // Step 4: حذف قراردادهای تسویه شده
        contracts.Item1 = _contractTransactionService.RemoveSettledAllContracts(new RemoveSettleAllContractsDataModel()
        {
            LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2),
            Contracts = contracts.Item1
        }).Contracts;


        // Step 5: محاسبه مجموع مبالغ تراکنش های وصولی قراردادها به تفکیک C13
        var dailyReceivedAmounts = _contractTransactionService.GetDailyReceivedAllContractsTransactionsAmounts(
            new GetReceivedAllContractsDataModel()
            {
                LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2)
            });

        // بهینه‌سازی: تبدیل لیست به Dictionary برای دسترسی سریع و جمع زدن مبالغ مشابه
        var receivedDict = dailyReceivedAmounts
            .GroupBy(x => x.C13)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(x => long.TryParse(x.Amount, out var val) ? val : 0)
            );

        // Step 6: کسر مبالغ تجمیعی از مانده بدهی قراردادها
        foreach (var contract in contracts.Item1)
        {
            if (receivedDict.TryGetValue(contract.C13, out var totalAmountToDeduct))
            {
                // کسر مبلغ وصولی از کل بدهی
                contract.TotalDebt -= totalAmountToDeduct;

                // اطمینان از عدم منفی شدن بدهی (بسته به قوانین بیزنس)
                if (contract.TotalDebt < 0)
                {
                    contract.TotalDebt = 0;
                }
            }
        }

        return contracts.Item1;

    }

    public async Task<CalculateRequestFacilityUnusedBalanceDto> CalculateRequestFacilityUnusedBalance(CalculateRequestFacilityUnusedBalanceDataModel dataModel, CancellationToken cancellationToken = default)
    {
        var getMosavabeNoByCaseIdDto = await _requestFacilityDatabaseService.GetMosavabeNoByCaseId(
            new GetMosavabeNoByCaseIdDataModel()
            { 
                CaseId = dataModel.CaseId.ToString(),
            });

        if (getMosavabeNoByCaseIdDto.MosavabeNumber == null)
            return new CalculateRequestFacilityUnusedBalanceDto()
            { 
                Amount = null,
                RemainingContractsCount = null 
            };

        var viewModel = new CalculateApprovalRequestUnusedBalanceVm()
        { 
            MosavabeNumber = getMosavabeNoByCaseIdDto.MosavabeNumber
        };

        var response = await _creditProcessService.CalculateApprovalRequestUnusedBalance(viewModel);
        return _mapper.Map<CalculateRequestFacilityUnusedBalanceDto>(response);
    }

    public async Task<PaymentPermissionIssuanceControlForLcpDto> PaymentPermissionIssuanceControlForLcp(PaymentPermissionIssuanceControlForLcpDataModel dataModel, CancellationToken cancellationToken = default)
    {
        var getMosavabeNoByCaseIdDto = await _requestFacilityDatabaseService.GetMosavabeNoByCaseId(
            new GetMosavabeNoByCaseIdDataModel()
            {
                CaseId = dataModel.CaseId.ToString(),
            });

        if (getMosavabeNoByCaseIdDto.MosavabeNumber == null)
            return new PaymentPermissionIssuanceControlForLcpDto()
            { Result = "404" };

        var getCustomerNoByMosavabeNoDto = await _creditProcessesDatabaseService
            .GetCustomerNoByMosavabeNo(new GetCustomerNoByMosavabeNoDataModel()
            {
                MosavabeNo = getMosavabeNoByCaseIdDto.MosavabeNumber
            }, cancellationToken);

        if (getCustomerNoByMosavabeNoDto.CustomerNo == null)
            return new PaymentPermissionIssuanceControlForLcpDto()
            { Result = "404" };

        var dm = new PaymentPermissionIssuanceControlForLcpDM()
        {
            MosavabeNumber = getMosavabeNoByCaseIdDto.MosavabeNumber,
            CustomerNumber = getCustomerNoByMosavabeNoDto.CustomerNo
        };

        return await _requestFacilityDatabaseService.PaymentPermissionIssuanceControlForLcp(dm);
    }

    public async Task<GetMosavabeExpireDateDto> GetMosavabeExpireDate(GetMosavabeExpireDateVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _requestFacilityDatabaseService.GetMosavabeExpireDate(viewModel, cancellationToken);
    }

    public async Task<SendApprovalLetterDto> SendApprovalLetter(SendApprovalLetterVm vm, CancellationToken cancellationToken = default)
    {

        return await _proxy.SendApprovalLetter(vm, cancellationToken);
    }
}