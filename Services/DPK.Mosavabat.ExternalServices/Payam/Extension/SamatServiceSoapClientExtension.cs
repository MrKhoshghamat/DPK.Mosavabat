using System.ServiceModel;
using System.Xml;
using DPK.Mosavabat.Models.Settings.Payam;
using Microsoft.Extensions.Options;
using PayamServices;

namespace DPK.Mosavabat.ExternalServices.Payam.Extension;

public static class SamatServiceSoapClientExtension
{
    public static SamatServiceSoapClient GetClient(
        IOptions<PayamServiceSettings> options)
    {
        var binding = new BasicHttpBinding
        {
            MaxBufferSize = int.MaxValue,
            MaxReceivedMessageSize = int.MaxValue,
            ReaderQuotas = XmlDictionaryReaderQuotas.Max,
            AllowCookies = true,
            Security =
            {
                Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                Transport =
                {
                    ClientCredentialType = HttpClientCredentialType.Basic
                }
            }
        };

        var endpointAddress = new EndpointAddress(options.Value.BaseUrl);
        var client = new SamatServiceSoapClient(binding, endpointAddress);

        client.ClientCredentials.UserName.UserName = options.Value.Username;
        client.ClientCredentials.UserName.Password = options.Value.Password;

        return client;
    }
}