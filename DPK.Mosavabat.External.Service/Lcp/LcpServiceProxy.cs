using System.Net;
using System.Net.Http.Headers;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.Lcp.Models.General;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DPK.Mosavabat.External.Services.Lcp;

public class LcpServiceProxy : ILcpServiceProxy
{
    private readonly ILoggerManager _logger;
    private readonly LcpServiceSetting _lcpServiceSetting;

    public LcpServiceProxy(IOptions<LcpServiceSetting> options, ILoggerManager logger)
    {
        _logger = logger;
        _lcpServiceSetting = options.Value;
    }

    private async Task<ApiServiceModel> ApiService<T>(string apiUrl, HttpMethod httpMethod, T requestData)
    {
        using var client = new HttpClient();

        var request = new HttpRequestMessage(httpMethod, apiUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(requestData), null, "application/json")
        };

        var token = await GetToken();

        if (token.UnAuthorized)
        {
            return ApiServiceModel.IsUnAuthorize;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        var response = await client.SendAsync(request);

        _logger.Trace(JsonConvert.SerializeObject(response));

        var result = await response.Content.ReadAsStringAsync();

        // --- استخراج پیام خطا از ساختار JSON سرویس LCP ---
        var lcpResponse = JsonConvert.DeserializeObject<LcpApiResponse>(result);

        // اگر خطا داشت، پیام خطا را به عنوان Result برگردان تا در CheckError شناسایی شود
        if (lcpResponse != null && !lcpResponse.Success)
        {
            var errorMessage = lcpResponse.Error?.Message ?? "خطای نامشخص";
            return ApiServiceModel.IsAuthorize(errorMessage);
        }

        return ApiServiceModel.IsAuthorize(result);
    }

    private async Task<TokenResponse> GetToken()
    {
        var apiUrl = _lcpServiceSetting.BaseUrl + "Service/Account/Login";
        var requestData = new GetTokenDataModel(_lcpServiceSetting.UserName, _lcpServiceSetting.Password, _lcpServiceSetting.ClientId);

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(requestData), null, "application/json")
        };
        var response = await client.SendAsync(request);
        _logger.Trace(JsonConvert.SerializeObject(response));

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return TokenResponse.IsUnAuthorized;
        }

        var token = JsonConvert.DeserializeObject<BaseResponse<TokenResponse>>(response.Content.ReadAsStringAsync().Result);

        return TokenResponse.IsAuthorized(token?.Result);
    }

    public async Task<string> AcceptAgent(AcceptAgentDataModel dataModel)
    {
        var apiUrl = _lcpServiceSetting.BaseUrl + "Service/Account/AcceptAgent";
        var response = await ApiService(apiUrl, HttpMethod.Post, dataModel);

        return response.Result;
    }

    public async Task<SendApprovalLetterDto> SendApprovalLetter(SendApprovalLetterVm approvalLetter, CancellationToken cancellationToken)
    {

        var apiUrl = _lcpServiceSetting.BaseUrl + "Service/FacilityApprovalLetter/Create";
        var response = await ApiService(apiUrl, HttpMethod.Post, approvalLetter);

        return new SendApprovalLetterDto()
        {
            Response = response.Result
        };
    }
}