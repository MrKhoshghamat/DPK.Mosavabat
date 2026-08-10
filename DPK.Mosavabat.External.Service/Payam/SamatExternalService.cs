using System.ServiceModel;
using System.Xml;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Payam;
using DPK.Mosavabat.Shared.Exceptions.ExternalServices;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Payam;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayamServices;

namespace DPK.Mosavabat.External.Services.Payam;

public class SamatExternalService : ISamatExternalService
{
    public SamatServiceSoap SamatServiceSoap { get; set; }

    private readonly ILoggerManager _logger;
    private string Url { get; }
    private string Username { get; }
    private string Password { get; }

    public SamatExternalService(ILoggerManager logger, IConfiguration configuration)
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

        var endpointAddress = new EndpointAddress(Url);
        var client = new SamatServiceSoapClient(binding, endpointAddress);

        client.ClientCredentials.UserName.UserName = Username;
        client.ClientCredentials.UserName.Password = Password;

        return client;
    }

    public async Task<EstelamCheckDto> EstelamCheck(EstelamCheckVm viewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await SamatServiceSoap.get_estelam_checkAsync(
                new get_estelam_checkRequest(
                    new get_estelam_checkRequestBody(
                        viewModel.NationalId)));

            _logger.Trace(JsonConvert.SerializeObject(response));

            var count = response.Body.get_estelam_checkResult.Length;
            return new EstelamCheckDto() { BouncedCheckCount = count.ToString() };
        }
        catch (Exception ex) when (
            ex is CommunicationException ||
            ex is EndpointNotFoundException ||
            ex is ServerTooBusyException ||
            ex is TimeoutException ||
            ex is HttpRequestException ||
            ex.InnerException is HttpRequestException)
        {
            _logger.Error($"Samat service unavailable: {ex}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.Error($"Samat service timeout: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (Exception ex)
        {
            _logger.Error($"Unexpected error calling Samat service: {ex.Message}");
            throw;
        }
    }

    public async Task<List<CbiTasDto>> GetCbiTas(CbiTasVm? viewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await SamatServiceSoap
                .get_estelam_tashilatAsync(new get_estelam_tashilatRequest(
                    new get_estelam_tashilatRequestBody(
                        viewModel.NationalId)));

            _logger.Trace(JsonConvert.SerializeObject(response));

            var cbiTasResult = response.Body.get_estelam_tashilatResult.CbiTas
                .ToList();

            return cbiTasResult.Select(cbiTas => new CbiTasDto()
                {
                    AdamSabtSanadEntezami = cbiTas.AdamSabtSanadEntezami,
                    AmBedehiKol = cbiTas.AmBedehiKol,
                    AmBenefit = cbiTas.AmBenefit,
                    AmDirkard = cbiTas.AmDirkard,
                    AmEltezam = cbiTas.AmEltezam,
                    AmMashkuk = cbiTas.AmMashkuk,
                    AmMoavagh = cbiTas.AmMoavagh,
                    AmOriginal = cbiTas.AmOriginal,
                    AmSarResid = cbiTas.AmSarResid,
                    AmSukht = cbiTas.AmSukht,
                    AmTahod = cbiTas.AmTahod,
                    ArzCode = cbiTas.ArzCode,
                    BadhesabiDate = cbiTas.BadhesabiDate,
                    BankCode = cbiTas.BankCode,
                    DasteBandi = cbiTas.DasteBandi,
                    Date = cbiTas.Date,
                    DateEstehal = cbiTas.DateEstehal,
                    DateSarResid = cbiTas.DateSarResid,
                    Estehal = cbiTas.Estehal,
                    GhrType = cbiTas.GhrType,
                    MainLName = cbiTas.MainLName,
                    Name = cbiTas.Name,
                    RequestNum = cbiTas.RequestNum,
                    RequstType = cbiTas.RequstType,
                    ShobeCode = cbiTas.ShobeCode,
                    ShobeName = cbiTas.ShobeName
                })
                .ToList();
        }
        catch (CommunicationException ex) when (
            ex is EndpointNotFoundException ||
            ex is ServerTooBusyException ||
            ex.InnerException is HttpRequestException ||
            ex.Message.Contains("Unable to connect") ||
            ex.Message.Contains("connection attempt failed"))
        {
            _logger.Error($"Samat service is unavailable: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.Error($"Samat service timeout: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (Exception ex)
        {
            _logger.Error($"Unexpected error calling Samat service: {ex.Message}");
            throw; // Re-throw non-connectivity issues as 500 or handle accordingly
        }
    }

    public async Task<HasNonCurrentDebtDto> HasNonCurrentDebt(CbiTasVm? viewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await SamatServiceSoap
                .get_estelam_tashilatAsync(
                    new get_estelam_tashilatRequest(
                        new get_estelam_tashilatRequestBody(
                            viewModel.NationalId)));

            _logger.Trace(JsonConvert.SerializeObject(response));

            var cbiTasResult = response.Body.get_estelam_tashilatResult.CbiTas
                .ToList();

            var bedehiKolList =
                (from cbiTas in cbiTasResult
                    where cbiTas.RequstType != 3 &&
                          cbiTas.AmBedehiKol == 1
                    select true)
                .ToList();

            var sarResid = cbiTasResult.Sum(x => x.AmSarResid);
            var moavagh = cbiTasResult.Sum(x => x.AmMoavagh);
            var mashkuk = cbiTasResult.Sum(x => x.AmMashkuk);

            if ((sarResid > 0 || moavagh > 0 || mashkuk > 0) || bedehiKolList.Contains(true))
                return new HasNonCurrentDebtDto() { HasNonCurrentDebt = true };

            return new HasNonCurrentDebtDto() { HasNonCurrentDebt = false };
        }
        catch (CommunicationException ex) when (
            ex is EndpointNotFoundException ||
            ex is ServerTooBusyException ||
            ex.InnerException is HttpRequestException ||
            ex.Message.Contains("Unable to connect") ||
            ex.Message.Contains("connection attempt failed"))
        {
            _logger.Error($"Samat service is unavailable: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.Error($"Samat service timeout: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (Exception ex)
        {
            _logger.Error($"Unexpected error calling Samat service: {ex.Message}");
            throw; // Re-throw non-connectivity issues as 500 or handle accordingly
        }
    }

    public async Task<List<BeneficiaryDto>> GetBeneficiaries(BeneficiaryVm viewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await SamatServiceSoap.GetEstelamAsync(
                new GetEstelamRequest(new GetEstelamRequestBody(viewModel.NationalCode, viewModel.BirthDate)));
            _logger.Trace(JsonConvert.SerializeObject(response));

            var zinafList = response.Body.GetEstelamResult.ZinafGroup
                .Where(b => b.NationalId != viewModel.NationalCode);

            return zinafList.Select(zinaf => new BeneficiaryDto
                {
                    Name = zinaf.Name,
                    NationalCode = zinaf.NationalId,
                    MainRelation = zinaf.MainRelationTypeTitle,
                    Relation = zinaf.RelationTypeTitle,
                    Jari = Convert.ToInt64(zinaf.JariAsl),
                    SarResid = Convert.ToInt64(zinaf.SarResidAsl),
                    Moavagh = Convert.ToInt64(zinaf.MoavaghAsl),
                    Mashkook = Convert.ToInt64(zinaf.MashkookAsl),
                    MandeTaahod = Convert.ToInt64(zinaf.MandeTaahod),
                    TotalDebt = Convert.ToInt64(zinaf.Total)
                })
                .ToList();
        }
        catch (CommunicationException ex) when (
            ex is EndpointNotFoundException ||
            ex is ServerTooBusyException ||
            ex.InnerException is HttpRequestException ||
            ex.Message.Contains("Unable to connect") ||
            ex.Message.Contains("connection attempt failed"))
        {
            _logger.Error($"Samat service is unavailable: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.Error($"Samat service timeout: {ex.Message}");
            throw new SamatExternalServiceUnavailableException("Samat", ex);
        }
        catch (Exception ex)
        {
            _logger.Error($"Unexpected error calling Samat service: {ex.Message}");
            throw; // Re-throw non-connectivity issues as 500 or handle accordingly
        }
    }
}