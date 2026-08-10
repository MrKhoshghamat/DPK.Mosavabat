using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.Lcp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;

namespace DPK.Mosavabat.External.Services.Lcp;

public class ServiceProvider : IServiceProvider
{
    private readonly Dictionary<string, string>? _errors;
    private readonly Dictionary<string, string>? _names;
    private readonly LcpServiceSetting _lcpServiceSetting;
    private readonly ILoggerManager _logger;
    private readonly string _lcpAdminBaseUrl;

    public ServiceProvider(IOptions<LcpServiceSetting> options, IConfiguration configuration, ILoggerManager logger)
    {
        _lcpServiceSetting = options.Value;
        _logger = logger;
        _lcpAdminBaseUrl = configuration["Lcp:adminBaseUrl"];
        _names = new Dictionary<string, string>
        {
            {"caseId", "شناسه مورد"},
            {"cuApprovalSubjectId", "شناسه موضوع"},
            {"facilityGroupCode", "کد نوع عقد"},
            {"contractNo", "شماره قرارداد"},
            {"brManagerName", "نام رئیس شعبه"},
            {"employeeCode", "کد پرسنلی رئیس شعبه"},
            {"contractDate", "تاریخ قرارداد"},
            {"branchCode", "کد شعبه"},
            {"contractSubject", "موضوع قرارداد"},
            {"contractAmount1Int", "کل بهای فروش مورد معامله به متقاضی به عدد"},
            {"contractAmount2Int", "قیمت تمام شده خرید مورد معامله توسط بانک به عدد"},
            {"benefitRate", "نرخ سود"},
            {"benefitAmountInt", "مبلغ سود قرارداد"},
            {"customerCashPriceInt", "مبلغ پیش پرداخت"},
            {"contractAmount3Int", "مابقی مبلغ"},
            {"refundWay", "نحوه بازپرداخت"},
            {"facilityLife", "دوره های اقساط"},
            {"countOfInstallments", "تعداد اقساط"},
            {"contractDueDate", "مدت قرارداد"},
            {"InterestRate", "نرخ وجهه التزام"},
            {"allDeposit", "لیست وثایق"},
            {"countOfContractVersion", "تعداد ورژن"},
            {"requestDate", "تاریخ قرارداد"},
            {"accountType", "نوع حساب"},
            {"accountNo", "شماره حساب"},
            {"collateralAmount", "مبلغ وثیقه"},
            {"introductionLetterNo", "شماره معرفی نامه" },
            {"introductionLetterDate", "تاریخ معرفی نامه" },
            {"documentAmountSum", "جمع مبلغ اسناد" },
            {"paymentAmountSum", "جمع مبلغ پرداختی سند" },
            {"contractAmount", "مبلغ اصل" }
        };
        _errors = new Dictionary<string, string>
        {
            {"Int", "نمی تواند صفر باشد "},
            {"String", "نمی تواند بدون مقدار یا خالی باشد "}
        };
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

    public async Task<string> AcceptAgentService(AcceptAgentDataModel acceptAgent)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/Account/AcceptAgent";
        var response = await ApiService(apiUrl, HttpMethod.Post, acceptAgent);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response.Result;
    }

    public async Task<string> AcceptRequestFacilityService(UpdateRequestFacilityDataModel updateRequestFacilityDataModel)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/RequestFacility/UpdateStatus";
        var response = await ApiService(apiUrl, HttpMethod.Put, updateRequestFacilityDataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response.Result;
    }

    public async Task<string> UpdateRadNumberRequestFacilityService(UpdateRequestFacilityRadNumber updateRequestFacilityRadNumber)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/RequestFacility/UpdateRadNumber";
        var response = await ApiService(apiUrl, HttpMethod.Put, updateRequestFacilityRadNumber);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response.Result;
    }

    public async Task<string> CreateTicketService(TicketDataModel ticketDataModel)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/Ticket/Create";
        var response = await ApiService(apiUrl, HttpMethod.Post, ticketDataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response.Result;
    }

    public async Task<string?> PrintContract(InsertContractDataModel insertContractDataModel)
    {
        var apiUrl = _lcpAdminBaseUrl + "Service/Mosavabat/Create";
        _logger.Trace(apiUrl);

        try
        {
            var contractModel = new ContractDataModel
            {
                CaseId = insertContractDataModel.CaseId,
                CUApprovalSubjectId = insertContractDataModel.CuApprovalSubjectId.ToString(),
                FacilityGroupCode = insertContractDataModel.FacilityGroupCode,
                RequestDocuments = insertContractDataModel.ContractDocuments,
                BranchManager = insertContractDataModel.BrManagerName,
                ContractDate = insertContractDataModel.ContractDate,
                BranchCode = insertContractDataModel.BranchCode,
                EmployeeCode = insertContractDataModel.EmployeeCode.ToString(),
                ContractSubject = insertContractDataModel.ContractSubject,
                ContractAmount1Int = insertContractDataModel.ContractAmount1Int,
                ContractAmount2Int = insertContractDataModel.ContractAmount2Int,
                BenefitRate = insertContractDataModel.BenefitRate,
                BenefitAmountInt = insertContractDataModel.BenefitAmountInt,
                CustomerCashPriceInt = insertContractDataModel.CustomerCashPriceInt,
                ContractAmount3Int = insertContractDataModel.ContractAmount3Int,
                RefundWay = insertContractDataModel.RefundWay,
                FacilityLife = insertContractDataModel.FacilityLife,
                CountOfInstallments = insertContractDataModel.CountOfInstallments.ToString(),
                ContractDueDate = insertContractDataModel.ContractDueDate,
                InterestRate = insertContractDataModel.InterestRate,
                AllDeposit = insertContractDataModel.AllDeposit,
                CountOfContractVersion = insertContractDataModel.CountOfContractVersion,
                RequestDate = insertContractDataModel.RequestDate,
                AccountType = insertContractDataModel.AccountType,
                AccountNo = insertContractDataModel.AccountNo,
                CollateralAmount = insertContractDataModel.CollateralAmount,
                ContractNo = insertContractDataModel.ContractNo,
                IntroductionLetterNo = insertContractDataModel.IntroductionLetterNo,
                IntroductionLetterDate = insertContractDataModel.IntroductionLetterDate,
                PaymentAmountOfDocument = long.Parse(insertContractDataModel.PaymentAmountSum),
                DocumentAmountSum = long.Parse(insertContractDataModel.DocumentAmountSum),
                PaymentAmountSum = long.Parse(insertContractDataModel.PaymentAmountSum),
                ContractAmount = long.Parse(insertContractDataModel.ContractAmount)
            };


            var response = await ApiService(apiUrl, HttpMethod.Post, contractModel);
            _logger.Trace(JsonConvert.SerializeObject(response));
            return response.Result;
        }
        catch (Exception e)
        {
            _logger.Trace(e.Message);
            throw;
        }

    }

    public async Task<string> GetBoardMembersListService(GetBoardMemberDataModel getBoardMemberDataModel)
    {
        var baseUrl = _lcpAdminBaseUrl + "Service/BoardMemberInfo/GetBoardMemberForMosavabat";
        var apiUrl = $"{baseUrl}?nationalCode={Uri.EscapeDataString(getBoardMemberDataModel.NationalCode)}";
        var response = await ApiService(apiUrl, HttpMethod.Get, getBoardMemberDataModel);
        
        _logger.Trace(JsonConvert.SerializeObject(response));
        return response.Result;
        
    }
}