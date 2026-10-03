using DPK.Mosavabat.Common.Enums.Customer;
using DPK.Mosavabat.Common.Exceptions;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.External.Services.Contracts.Bsi;
using DPK.Mosavabat.Shared.DataModels.Bsi;
using System.Net;
using Microsoft.Extensions.Options;
using RestSharp;

namespace DPK.Mosavabat.External.Services.Bsi;

internal class CustomerBsiExternalService : ICustomerBsiExternalService
{
    private static AuthDataModel? _token;
    private readonly BsiServiceSetting _setting;

    public CustomerBsiExternalService(IOptions<BsiServiceSetting> options)
    {
        _setting = options.Value;
    }

    public async Task<List<CustomerBsiModel>?> GetCustomer(BsiCustomerDataModel dm, CancellationToken cancellationToken)
    {
        var parameters = new List<string>
        {
            "page=0",
            "size=1000"
        };

        switch (dm.CustomerType)
        {
            case CustomerType.Legal when dm.LegalNationalId.HasValue:
                parameters.Add($"corporateCustomerIdNumber.equals={dm.LegalNationalId}");
                break;

            case CustomerType.Real when dm.NationalCode.HasValue:
                parameters.Add($"nationalIdNumber.equals={dm.NationalCode}");
                break;

            case CustomerType.Foreigners when dm.NationalCode.HasValue:
                parameters.Add($"foreignerIdNumber.equals={dm.NationalCode.Value.ToFixLengthWithZero(15)}");
                break;
            default:
                throw new DpkException();
        }

        if (dm.CustomerNumber.HasValue)
        {
            parameters.Add($"customerNumber.equals={dm.CustomerNumber}");
        }

        var customerResult = await SendDataAndGetResult<List<CustomerBsiModel>>(parameters, _setting.CustomerService);

        customerResult = customerResult?
            .Where(c => c.CustomerType != 24
                        && c.DeletedCustomerFlag.Id == 0)
            .OrderByDescending(c => c.CustomerDefinedDate).ToList();

        if (customerResult == null) throw new DpkException("امکان دریافت اطلاعات مقدور نمی باشد.لطفا چند دقیقه دیگر تلاش نمایید");

        foreach (var cu in customerResult.OrEmptyIfNull())
        {
            cu.TrimStringProperties();
        }

        return customerResult;

    }

    public async Task<IEnumerable<BalanceBsiDataModel>?> GetAllBalances(BsiBalanceRequestDataModel dm, CancellationToken cancellationToken)
    {
        var queryParams = new List<string> { "page=0", "size=1000" };
        if (dm.AccountNumber != null)
            queryParams.Add($"accountNumber.equals={dm.AccountNumber}");
        if (dm.CustomerNumber != null)
            queryParams.Add($"customerNumber.equals={dm.CustomerNumber}");

        var balanceResult = await SendDataAndGetResult<IEnumerable<BalanceBsiDataModel>>(queryParams, _setting.BalanceService);
        return balanceResult == null ? null :
            TransformBalanceData(balanceResult);
    }

    public async Task<IEnumerable<BalanceBsiDataModel>?> GetBalance(BsiBalanceRequestDataModel dm, CancellationToken cancellationToken)
    {
        var balanceResult = await GetAllBalances(dm, cancellationToken);

        if (dm.FilterAccount)
        {
            balanceResult = balanceResult?.Where(x => x.AccountStatus.Id == 2);
        }

        return balanceResult != null ? TransformBalanceData(balanceResult) : null;
    }

    public async Task<List<BranchBsiDataModel>?> GetBranches(PagingDataModel dm, CancellationToken cancellationToken)
    {
        var result = await SendDataAndGetResult<List<BranchBsiDataModel>>(dm.GetPageParametersForBsi(),
            _setting.BranchService);

        return result;
    }

    private async Task<T?> SendDataAndGetResult<T>(List<string> parameters, string serviceUrl)
        where T : class
    {
        var queryString = string.Join("&", parameters);
        var customerServiceUrl = $"{serviceUrl}?{queryString}";
        var customerResult = await Get<T>(customerServiceUrl);
        return customerResult;
    }

    private async Task<T?> Get<T>(string request) where T : class
    {
        try
        {
            var response = await CallGet<T>(request);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                response = await CallGet<T>(request, true);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new DpkException("امکان دریافت اطلاعات مقدور نمی باشد.لطفا چند دقیقه دیگر تلاش نمایید");
            }

            return response.Data;
        }
        catch (Exception)
        {
            throw new DpkException("امکان دریافت اطلاعات مقدور نمی باشد.لطفا چند دقیقه دیگر تلاش نمایید");
        }
    }

    private async Task<RestResponse<T>> CallGet<T>(string resource, bool isNeedToRefresh = false) where T : notnull
    {
        var client = await GetRestClient(resource, await GetToken(isNeedToRefresh), Method.Get);
        return await client.Item1.ExecuteAsync<T>(client.Item2);
    }

    private async Task<T?> Post<T>(string resource, object model, string? token = null) where T : class
    {
        var client = await GetRestClient(resource, token, Method.Post);
        client.Item2.AddBody(model);
        var result = await client.Item1.ExecuteAsync<T>(client.Item2);
        return result.Data;
    }

    private Task<(RestClient, RestRequest)> GetRestClient(string resource, string? token, Method method)
    {
        var baseBsi = _setting.BaseUrl;

        var option = new RestClientOptions()
        {
            BaseUrl = new Uri(baseBsi),
            RemoteCertificateValidationCallback = (_, _, _, _) => true,
        };

        var client = new RestClient(option);

        var request = new RestRequest(resource, method)
        {
            RequestFormat = DataFormat.Json
        };
        AddHeaderToken(request, token);
        return Task.FromResult((client, request));
    }

    private static void AddHeaderToken(RestRequest request, string? token = null)
    {
        if (token != null)
            request.AddHeader("Authorization", $"Bearer {token}");
    }

    private async Task<string?> GetToken(bool isNeedToRefresh = false)
    {
        if (_token != null && !_token.IsExpire() && !isNeedToRefresh) return _token.Access_token;
        var username = _setting.Username;
        var password = _setting.Password;
        var login = _setting.Login
            ;
        var queryResult = await Post<AuthDataModel>($"{login}", new
        {
            username,
            password
        });

        _token = queryResult;

        if (_token?.Access_token != null)
            return _token.Access_token;

        _token = null;
        throw new DpkException("امکان دریافت اطلاعات مقدور نمی باشد.لطفا چند دقیقه دیگر تلاش نمایید");
    }

    private static IEnumerable<BalanceBsiDataModel> TransformBalanceData(IEnumerable<BalanceBsiDataModel> balances)
    {
        var balanceBsiDataModels = balances as BalanceBsiDataModel[] ?? balances.ToArray();
        foreach (var balance in balanceBsiDataModels)
        {
            balance.TrimStringProperties();
        }
        return balanceBsiDataModels;
    }
}