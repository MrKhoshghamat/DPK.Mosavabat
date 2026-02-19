using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.UpdatePaymentPermission;

/// <summary>
/// هندلر سرویس بروز رسانی مجوز پرداخت | حذف یا ابطال 
/// </summary>
internal class UpdatePaymentPermissionCommandHandler(
    ILmsExternalService lmsExternalService,
    ICreditProcessSqlService creditProcessSqlService,
    IMapper mapper)
    : ICommandHandler<
        UpdatePaymentPermissionCommand,
        UpdatePaymentPermissionDto>
{
    public async Task<UpdatePaymentPermissionDto> Handle(
        UpdatePaymentPermissionCommand command,
        CancellationToken cancellationToken)
    {
        await lmsExternalService
            .UpdatePaymentPermission(
                mapper.Map<UpdatePaymentPermissionDm>(command),
                cancellationToken);

        var updatePaymentPermissionResponse = await lmsExternalService.UpdatePaymentPermission(
            mapper.Map<UpdatePaymentPermissionDm>(command),
            cancellationToken);

        if (command.IsCanceled)
        {
            await creditProcessSqlService.UpdateIsDeletedCuContract(
                new UpdateIsDeletedCuContractDm(
                    command.Id,
                    command.IsCanceled),
                cancellationToken);
        }

        return updatePaymentPermissionResponse;
    }
}