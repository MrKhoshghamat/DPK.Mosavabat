using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lms;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DPK.Mosavabat.External.Services.Lms;

public class LmsServiceProxy : ILmsServiceProxy
{
    private readonly ILoggerManager _logger;
    private readonly LmsServiceSetting _lmsServiceSetting;

    public LmsServiceProxy(IOptions<LmsServiceSetting> options, ILoggerManager logger)
    {
        _logger = logger;
        _lmsServiceSetting = options.Value;
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

    private async Task<TokenResponse> GetToken()
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/Token/login";
        var requestData = new GetTokenDataModel(_lmsServiceSetting.UserName, _lmsServiceSetting.Password, _lmsServiceSetting.ClientId);

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

    public async Task<BaseResponse<PaymentPermissionIssuanceControlDto>> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/PaymentPermissionIssuanceControl";
        var response = await ApiService(apiUrl, HttpMethod.Post, dataModel);

        return GetResponse<PaymentPermissionIssuanceControlDto>(response);
    }

    public async Task<BaseResponse<LmsResponse>> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/CheckAndUpdateC13InPaymentPermission";
        var response = await ApiService(apiUrl, HttpMethod.Post, dataModel);

        _logger.Trace(JsonConvert.SerializeObject(response));

        return GetResponse<LmsResponse>(response);

    }

    public async Task<BaseResponse<LmsResponse>> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/InsertPaymentPermission";
        var response = await ApiService(apiUrl, HttpMethod.Post, dataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));

        return GetResponse<LmsResponse>(response);
    }

    public async Task<BaseResponse<LmsResponse>> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/UpdatePaymentPermission";
        var response = await ApiService(apiUrl, HttpMethod.Put, dataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));

        return GetResponse<LmsResponse>(response);
    }

    public async Task<BaseResponse<List<GetPaymentPermissionDto>>> FetchPaymentPermissions(GetPaymentPermissionListDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/FetchPaymentPermissions";
        var response = await ApiService(apiUrl, HttpMethod.Post, dataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));

        return GetResponse<List<GetPaymentPermissionDto>>(response);
    }

    public async Task<GetIsCanceledPaymentPermissionsDto> FetchIsCanceledPaymentPermissions()
    {
        var apiUrl = _lmsServiceSetting + "Service/CreditProcessApplication/FetchIsCanceledPaymentPermissions";

        using var client = new HttpClient();

        var response = await client.GetAsync(apiUrl);

        var result = await response.Content.ReadAsStringAsync();

        _logger.Trace(JsonConvert.SerializeObject(response));

        return JsonConvert.DeserializeObject<GetIsCanceledPaymentPermissionsDto>(result);
    }

    public async Task<BaseResponse<BoundaryComplianceInquiryDto>> CheckBoundaryComplianceInquiry(BoundaryComplianceInquiryDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/BoundaryComplianceInquiry";
        var response = await ApiService(apiUrl,HttpMethod.Post,dataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return GetResponse<BoundaryComplianceInquiryDto>(response);

    }

    public async Task<BaseResponse<LmsResponse>> RegisterPaymentPermissionExceptionReason(RegisterPaymentPermissionExceptionReasonsDataModel dataModel)
    {
       var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/RegisterPaymentPermissionExceptionReason";
       var response = await ApiService(apiUrl,HttpMethod.Post, dataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return GetResponse<LmsResponse>(response);
    }

    public async Task<BaseResponse<List<GetPaymentPermissionExceptionReasonsDto>>> GetPaymentPermissionExceptionReasons(GetPaymentPermissionExceptionReasonsDataModel dataModel)
    {
        var apiUrl = _lmsServiceSetting.BaseUrl + "Service/CreditProcessApplication/FetchPaymentPermissionExceptionReasons";
        var response = await ApiService(apiUrl, HttpMethod.Post, dataModel);
        _logger.Trace(JsonConvert.SerializeObject(response));
        return GetResponse<List<GetPaymentPermissionExceptionReasonsDto>>(response);
    }


    private static BaseResponse<T> GetResponse<T>(ApiServiceModel response) where T : class
    {
        if (!response.IsAuthorized)
        {
            return new BaseResponse<T>()
            {
                IsAuthenticate = response.IsAuthorized
            };
        }

        var responseModel = JsonConvert.DeserializeObject<BaseResponse<T>>(response.Result);
        responseModel.IsAuthenticate = response.IsAuthorized;
        return responseModel;
    }

}