using System.ServiceModel;
using System.Xml;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayamServices;

namespace DPK.Mosavabat.External.Services.Payam;

public class SamatServices : ISamatServices
{
    public SamatServiceSoap SamatServiceSoap { get; set; }

    private readonly ILoggerManager _logger;
    private string Url { get; }
    private string Username { get; }
    private string Password { get; }

    public SamatServices(ILoggerManager logger, IConfiguration configuration)
    {
        Url = configuration["Payam:SamatServiceSoapUrl"];
        Username = configuration["Payam:Username"];
        Password = configuration["Payam:Password"];
        SamatServiceSoap = GetClient();
        _logger = logger;
    }

    private SamatServiceSoapClient GetClient()
    {
        var binding = new BasicHttpBinding
        {
            MaxBufferSize = int.MaxValue,
            MaxReceivedMessageSize = int.MaxValue,
            ReaderQuotas = XmlDictionaryReaderQuotas.Max,
            AllowCookies = true
        };

        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        var endpointAddress = new EndpointAddress(Url);
        var client = new SamatServiceSoapClient(binding, endpointAddress);

        client.ClientCredentials.UserName.UserName = Username;
        client.ClientCredentials.UserName.Password = Password;

        return client;
    }

    public async Task<string> EstelamCheck(EstelamCheckDataModel dataModel)
    {
        var response = await SamatServiceSoap.get_estelam_checkAsync(new get_estelam_checkRequest(new get_estelam_checkRequestBody(dataModel.NationalId)));
        _logger.Trace(JsonConvert.SerializeObject(response));
        var count = response.Body.get_estelam_checkResult.Length;
        return count.ToString();
    }
}