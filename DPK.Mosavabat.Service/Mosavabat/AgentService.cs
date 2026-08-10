using System.Xml;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Mosavabat;

public class AgentService : IAgentService
{
    private readonly IUserDatabaseService _userDatabaseService;
    private readonly IAgentDatabaseService _agentDatabaseService;
    private readonly IWorkflowEngineServices _workflowEngineServices;
    private readonly ILoggerManager _logger;

    public AgentService(
        IUserDatabaseService userDatabaseService,
        IWorkflowEngineServices workflowEngineServices,
        ILoggerManager logger,
        IAgentDatabaseService agentDatabaseService)
    {
        _userDatabaseService = userDatabaseService;
        _workflowEngineServices = workflowEngineServices;
        _logger = logger;
        _agentDatabaseService = agentDatabaseService;
    }

    public async Task<RegisterAgentDto?> RegisterAgentAsync(RegisterAgentVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var getUserNameResult = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = Convert.ToInt32(viewModel.BranchCode),
            MosavabatClassName = MosavabatClassName.AgentRegistry
        }, cancellationToken);

        if (string.IsNullOrEmpty(getUserNameResult.Username))
            return new RegisterAgentDto() { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };

        var mosavabatAgentDataModel = new MosavabatAgentDataModel()
        {
            BranchCode = viewModel.BranchCode,
            CreationDate = viewModel.CreationDate,
            LastName = viewModel.LastName,
            LegalEmail = viewModel.LegalEmail,
            LegalName = viewModel.LegalName,
            LegalNationalId = viewModel.LegalNationalId,
            Mobile = viewModel.Mobile,
            Name = viewModel.Name,
            NationalCode = viewModel.NationalCode
        };

        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(mosavabatAgentDataModel, getUserNameResult.Username, "AgentRegister");

        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "AgentRegistry");

        var xDoc = new XmlDocument();

        xDoc.LoadXml(data);

        var response = await _workflowEngineServices.CreateCasesAsync(xDoc);

        return new RegisterAgentDto() { MosavabatResponseDataModel = response };
    }

    public async Task<CheckAgentRegistryExistsDto> CheckAgentRegistryExists(CheckAgentRegistryExistsDm dataModel, CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var response = await _agentDatabaseService.CheckAgentRegistryExists(dataModel, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(response));

        return response;
    }

    public async Task<RemoveAgentFromCartableDto> RemoveAgentFromCartable(
    AbortCasesVm viewModel,
    CancellationToken cancellationToken = default)
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
                return new RemoveAgentFromCartableDto
                {
                    IsRemoved = false,
                    ErrorMessage = "امکان حذف نماینده از کارتابل وجود ندارد."
                };
            }

            return new RemoveAgentFromCartableDto
            {
                IsRemoved = true
            };
        }
        catch (XmlException)
        {
            _logger.Error(
                $"Invalid AbortCases XML. CaseId={viewModel.Cases.Enities[0].radNumber}");

            return new RemoveAgentFromCartableDto
            {
                IsRemoved = false,
                ErrorMessage = "خطا در ساختار داده ارسالی به موتور فرایند"
            };
        }
        catch (OperationCanceledException)
        {
            _logger.Warn(
                $"Abort case operation cancelled. CaseId={viewModel.Cases.Enities[0].radNumber}");

            return new RemoveAgentFromCartableDto
            {
                IsRemoved = false,
                ErrorMessage = "عملیات لغو شد"
            };
        }
        catch (Exception)
        {
            _logger.Error(
                $"Unexpected error while aborting Bizagi case. CaseId={viewModel.Cases.Enities[0].radNumber}");

            return new RemoveAgentFromCartableDto
            {
                IsRemoved = false,
                ErrorMessage = "خطای سیستمی در حذف پرونده از کارتابل"
            };
        }
    }


    public async Task<UpdateCaseIdAndRadNumberInAgentRegistryDto> UpdateCaseIdAndRadNumberInAgentRegistry(UpdateCaseIdAndRadNumberInAgentRegistryVm viewModel, CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var dataModel = new UpdateCaseIdAndRadNumberInAgentRegistryDm()
        {
            Agent = viewModel.Agent,
            CaseId = viewModel.CaseId,
            RadNumber = viewModel.RadNumber
        };

        var response = await _agentDatabaseService.UpdateCaseIdAndRadNumberInAgentRegistry(dataModel, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(response));

        return response;
    }

    public async Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _agentDatabaseService.GetRadNumberByCaseId(viewModel, cancellationToken);
    }

    public async Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _agentDatabaseService.GetUserNameByCaseId(viewModel, cancellationToken);
    }

    public async Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default)
    {
        return await _agentDatabaseService.GetUserIdByUserName(viewModel, cancellationToken);
    }
    public async Task LcpAgentAbortCase(int caseId, CancellationToken cancellationToken)
    {
        await _agentDatabaseService.LcpAgentAbortCase(caseId, cancellationToken);
    }
}
