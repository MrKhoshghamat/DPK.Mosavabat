using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetPaymentPermissions;

/// <summary>
/// هندلر سرویس دریافت مجوزهای پرداخت مصوبه
/// </summary>
internal class GetPaymentPermissionsQueryHandler(
    ILmsExternalService lmsExternalService,
    IMapper mapper)
    : IQueryHandler<
        GetPaymentPermissionsQuery,
        List<PaymentPermissionsDto>>
{
    public async Task<List<PaymentPermissionsDto>> Handle(
        GetPaymentPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        return await lmsExternalService.FetchPaymentPermissions(
            mapper.Map<FetchPaymentPermissionsDm>(query),
            cancellationToken);
    }
}