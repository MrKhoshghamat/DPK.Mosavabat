using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.ExternalServices.Lms.Contract;

public interface ILmsExternalService
    : ITransientDependency
{
    Task<InsertPaymentPermissionDto> InsertPaymentPermission(
        InsertPaymentPermissionDm dm,
        CancellationToken cancellationToken = default);

    Task<UpdatePaymentPermissionDto> UpdatePaymentPermission(
        UpdatePaymentPermissionDm dm,
        CancellationToken cancellationToken = default);

    Task<List<PaymentPermissionsDto>> FetchPaymentPermissions(
        FetchPaymentPermissionsDm dm,
        CancellationToken cancellationToken = default);

    Task<List<PaymentPermissionsDto>> FetchIsCanceledPaymentPermissions(
        CancellationToken cancellationToken = default);
}