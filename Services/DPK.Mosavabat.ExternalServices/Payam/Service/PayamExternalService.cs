using DPK.Mosavabat.ExternalServices.Payam.Contract;
using DPK.Mosavabat.Models.DataModels.Payam;
using DPK.Mosavabat.Models.EntitiesDto.Payam;
using Microsoft.Extensions.Options;
using PayamServices;
using DPK.Mosavabat.ExternalServices.Payam.Extension;
using DPK.Mosavabat.Models.Settings.Payam;

namespace DPK.Mosavabat.ExternalServices.Payam.Service;

public class PayamExternalService(
    IOptions<PayamServiceSettings> options)
    : IPayamExternalService
{
    private readonly SamatServiceSoap _samatServiceSoap = SamatServiceSoapClientExtension
        .GetClient(options);

    public async Task<CheckInquiryDto> CheckInquiry(
        CheckInquiryDm dm,
        CancellationToken cancellationToken = default)
    {
        var response = await _samatServiceSoap.get_estelam_checkAsync(
            new get_estelam_checkRequest(
                new get_estelam_checkRequestBody(
                    dm.NationalId)));

        var count = response.Body.get_estelam_checkResult.Length;
        return new CheckInquiryDto(
            count.ToString());
    }
}