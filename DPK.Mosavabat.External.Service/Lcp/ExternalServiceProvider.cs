using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.ApiService;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.Lcp.Models;
using DPK.Mosavabat.Shared.ViewModels.ApiService;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Options;

namespace DPK.Mosavabat.External.Services.Lcp;

public class ExternalServiceProvider : IExternalServiceProvider
{
    private readonly ILoggerManager _logger;
    private readonly string _lcpAdminBaseUrl;
    private readonly LcpServiceSetting _lcpServiceSetting;

    public ExternalServiceProvider(IOptions<LcpServiceSetting> options, IConfiguration configuration, ILoggerManager logger)
    {
        _lcpServiceSetting = options.Value;
        _logger = logger;
        _lcpAdminBaseUrl = configuration["Lcp:adminBaseUrl"];
    }

    private async Task<ApiServiceModel> ApiService<T>(string apiUrl, HttpMethod httpMethod, T requestData)
    {
        using var client = new HttpClient();

        var request = new HttpRequestMessage(httpMethod, apiUrl);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestData), null, "application/json");

        var token = await GetToken();

        if (token is null || token.UnAuthorized)
        {
            return ApiServiceModel.IsUnAuthorize;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        var response = await client.SendAsync(request);

        _logger.Trace(JsonConvert.SerializeObject(response));

        var result = await response.Content.ReadAsStringAsync();

        return ApiServiceModel.IsAuthorize(result);

    }

    public async Task<string> ApiServiceWithoutAuth<T>(string apiUrl, HttpMethod httpMethod, T requestData)
    {
        using var client = new HttpClient();

        var request = new HttpRequestMessage(httpMethod, apiUrl);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestData), null, "application/json");

        var response = await client.SendAsync(request);

        var result = await response.Content.ReadAsStringAsync();

        return result;
    }
    private async Task<TokenResponse> GetToken()
    {
        var apiUrl = _lcpServiceSetting.BaseUrl + "Service/Account/Login";
        var requestData = new GetTokenDataModel(_lcpServiceSetting.UserName, _lcpServiceSetting.Password, _lcpServiceSetting.ClientId);

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestData), null, "application/json");
        var response = await client.SendAsync(request);
        _logger.Trace(JsonConvert.SerializeObject(response));

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return TokenResponse.IsUnAuthorized;
        }

        var token = JsonConvert.DeserializeObject<BaseResponse<TokenResponse>>(response.Content.ReadAsStringAsync().Result);

        return TokenResponse.IsAuthorized(token.Result);
    }

    public async Task<AcceptAgentDto> AcceptAgentService(AcceptAgentVm viewModel, CancellationToken cancellationToken)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/Account/AcceptAgent";

        var apiServiceVm = new ApiServiceVm<AcceptAgentVm>()
        {
            ApiUrl = apiUrl,
            HttpMethod = HttpMethod.Post,
            RequestData = viewModel
        };

        var apiResponse = await ApiService(apiServiceVm.ApiUrl, apiServiceVm.HttpMethod, apiServiceVm.RequestData);

        _logger.Trace(JsonConvert.SerializeObject(apiResponse));

        return new AcceptAgentDto() { Response = apiResponse.Result };
    }

    public async Task<UpdateRequestFacilityDto> AcceptRequestFacilityService(UpdateRequestFacilityVm viewModel, CancellationToken cancellationToken)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/RequestFacility/UpdateStatus";

        var apiServiceVm = new ApiServiceVm<UpdateRequestFacilityVm>()
        {
            ApiUrl = apiUrl,
            HttpMethod = HttpMethod.Put,
            RequestData = viewModel
        };

        var apiResponse = await ApiService(apiServiceVm.ApiUrl, apiServiceVm.HttpMethod, apiServiceVm.RequestData);

        _logger.Trace(JsonConvert.SerializeObject(apiResponse));

        return new UpdateRequestFacilityDto() { Response = apiResponse.Result };
    }

    public async Task<UpdateRequestFacilityRadNumberDto> UpdateRadNumberRequestFacilityService(UpdateRequestFacilityRadNumberVm viewModel,
        CancellationToken cancellationToken)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/RequestFacility/UpdateRadNumber";

        var apiServiceVm = new ApiServiceVm<UpdateRequestFacilityRadNumberVm>()
        {
            ApiUrl = apiUrl,
            HttpMethod = HttpMethod.Put,
            RequestData = viewModel
        };

        var apiResponse = await ApiService(apiServiceVm.ApiUrl, apiServiceVm.HttpMethod, apiServiceVm.RequestData);

        _logger.Trace(JsonConvert.SerializeObject(apiResponse));

        return new UpdateRequestFacilityRadNumberDto() { Response = apiResponse.Result };
    }

    public async Task<InsertContractDto?> PrintContract(InsertContractVm viewModel, CancellationToken cancellationToken)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/Mosavabat/Create";

        var contractDataModel = new ContractDataModel()
        {
            CaseId = viewModel.CaseId,
            CUApprovalSubjectId = viewModel.CuApprovalSubjectId.ToString(),
            FacilityGroupCode = viewModel.FacilityGroupCode,
            RequestDocuments = viewModel.ContractDocuments,
            BranchManager = viewModel.BrManagerName,
            ContractDate = viewModel.ContractDate,
            BranchCode = viewModel.BranchCode,
            EmployeeCode = viewModel.EmployeeCode.ToString(),
            ContractSubject = viewModel.ContractSubject,
            ContractAmount1Int = viewModel.ContractAmount1Int,
            ContractAmount2Int = viewModel.ContractAmount2Int,
            BenefitRate = viewModel.BenefitRate,
            BenefitAmountInt = viewModel.BenefitAmountInt,
            CustomerCashPriceInt = viewModel.CustomerCashPriceInt,
            ContractAmount3Int = viewModel.ContractAmount3Int,
            RefundWay = viewModel.RefundWay,
            FacilityLife = viewModel.FacilityLife,
            CountOfInstallments = viewModel.CountOfInstallments.ToString(),
            ContractDueDate = viewModel.ContractDueDate,
            InterestRate = viewModel.InterestRate,
            AllDeposit = viewModel.AllDeposit,
            CountOfContractVersion = viewModel.CountOfContractVersion,
            RequestDate = viewModel.RequestDate,
            AccountType = viewModel.AccountType,
            AccountNo = viewModel.AccountNo,
            CollateralAmount = viewModel.CollateralAmount,
            ContractNo = viewModel.ContractNo,
            IntroductionLetterNo = viewModel.IntroductionLetterNo,
            IntroductionLetterDate = viewModel.IntroductionLetterDate,
            PaymentAmountOfDocument = long.Parse(viewModel.PaymentAmountSum),
            DocumentAmountSum = long.Parse(viewModel.DocumentAmountSum),
            PaymentAmountSum = long.Parse(viewModel.PaymentAmountSum),
            ContractAmount = long.Parse(viewModel.ContractAmount)
        };

        var apiServiceVm = new ApiServiceVm<ContractDataModel>()
        {
            ApiUrl = apiUrl,
            HttpMethod = HttpMethod.Post,
            RequestData = contractDataModel
        };

        var apiResponse = await ApiService(apiServiceVm.ApiUrl, apiServiceVm.HttpMethod, apiServiceVm.RequestData);

        _logger.Trace(JsonConvert.SerializeObject(apiResponse));

        return new InsertContractDto() { Response = apiResponse.Result };
    }

    public async Task<TicketDto> CreateTicketService(TicketVm viewModel, CancellationToken cancellationToken)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/Ticket/Create";

        var apiServiceVm = new ApiServiceVm<TicketVm>()
        {
            ApiUrl = apiUrl,
            HttpMethod = HttpMethod.Post,
            RequestData = viewModel
        };

        var apiResponse = await ApiService(apiServiceVm.ApiUrl, apiServiceVm.HttpMethod, apiServiceVm.RequestData);
        _logger.Trace(JsonConvert.SerializeObject(apiResponse));
        return new TicketDto() { Response = apiResponse.Result };
    }
}