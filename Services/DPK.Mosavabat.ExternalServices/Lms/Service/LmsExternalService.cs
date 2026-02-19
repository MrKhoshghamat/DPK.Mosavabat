using AutoMapper;
using DPK.Mosavabat.ExternalServices.Extensions;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.ExternalServices.Lms.Proxy;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.Proxies.Lms;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.ExternalServices.Lms.Service;

public class LmsExternalService(
    ILmsProxyService lmsProxyService,
    IMapper mapper)
    : ILmsExternalService
{
    private const string ServiceName = "LmsExternalService";

    public async Task<InsertPaymentPermissionDto> InsertPaymentPermission(
        InsertPaymentPermissionDm dm,
        CancellationToken cancellationToken = default)
    {
        var response = await lmsProxyService.InsertPaymentPermissionAsync(
            mapper.Map<InsertPaymentPermissionProxyDm>(dm),
            cancellationToken);

        return response.GetContentOrThrow(ServiceName);
    }

    public async Task<UpdatePaymentPermissionDto> UpdatePaymentPermission(
        UpdatePaymentPermissionDm dm,
        CancellationToken cancellationToken = default)
    {
        var response = await lmsProxyService.UpdatePaymentPermissionAsync(
            mapper.Map<UpdatePaymentPermissionProxyDm>(dm),
            cancellationToken);

        return response.GetContentOrThrow(ServiceName);
    }

    public async Task<List<PaymentPermissionsDto>> FetchPaymentPermissions(
        FetchPaymentPermissionsDm dm,
        CancellationToken cancellationToken = default)
    {
        var response = await lmsProxyService.FetchPaymentPermissionsAsync(
            mapper.Map<FetchPaymentPermissionsProxyDm>(dm),
            cancellationToken);

        return response.GetContentOrThrow(ServiceName);
    }

    public async Task<List<PaymentPermissionsDto>> FetchIsCanceledPaymentPermissions(
        CancellationToken cancellationToken = default)
    {
        var response = await lmsProxyService.FetchIsCanceledPaymentPermissionsAsync(
            cancellationToken);

        return response.GetContentOrThrow(ServiceName);
    }
}