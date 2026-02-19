using DPK.Mosavabat.Common.Consts.Proxies.Lms;
using DPK.Mosavabat.Models.DataModels.Proxies.Lms;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using Refit;

namespace DPK.Mosavabat.ExternalServices.Lms.Proxy;

public interface ILmsProxyService
{
    [Post(LmsProxyConsts.Urls.InsertPaymentPermission)]
    Task<ApiResponse<InsertPaymentPermissionDto>> InsertPaymentPermissionAsync(
        [Body] InsertPaymentPermissionProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LmsProxyConsts.Urls.UpdatePaymentPermission)]
    Task<ApiResponse<UpdatePaymentPermissionDto>> UpdatePaymentPermissionAsync(
        [Body] UpdatePaymentPermissionProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LmsProxyConsts.Urls.FetchPaymentPermissions)]
    Task<ApiResponse<List<PaymentPermissionsDto>>> FetchPaymentPermissionsAsync(
        [Body] FetchPaymentPermissionsProxyDm dm,
        CancellationToken cancellationToken = default);

    [Post(LmsProxyConsts.Urls.FetchIsCanceledPaymentPermissions)]
    Task<ApiResponse<List<PaymentPermissionsDto>>> FetchIsCanceledPaymentPermissionsAsync(
        CancellationToken cancellationToken = default);
}