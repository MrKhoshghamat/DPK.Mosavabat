using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Factory.Contract;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.RemoveRequestFromCartable;

/// <summary>
/// هندلر سرویس حذف درخواست از کارتابل
/// </summary>
internal class RemoveRequestFromCartableCommandHandler(
    IRequestFacilitySqlService requestFacilitySqlService,
    IWorkflowEngineService workflowEngineService,
    IRequestFacilityFactory requestFacilityFactory)
    : ICommandHandler<
        RemoveRequestFromCartableCommand,
        Result<RemoveRequestFromCartableDto>>
{
    public Task<Result<RemoveRequestFromCartableDto>> Handle(
        RemoveRequestFromCartableCommand command,
        CancellationToken cancellationToken)
    {
        return requestFacilitySqlService
            // 1️⃣ Load Username
            .GetUserNameByCaseId(
                new GetUserNameByCaseIdDm(command.CaseId),
                cancellationToken)

            .Map(dm => dm.Username)

            // 2️⃣ Load UserId
            .Bind(username =>
                requestFacilitySqlService.GetUserIdByUsername(
                        new GetUserIdByUsernameDm(username),
                        cancellationToken)
                    .Map(dm => (username, dm.UserId)))

            // 3️⃣ Load RadNumber
            .Bind(x =>
                requestFacilitySqlService.GetRadNumberByCaseId(
                        new GetRadNumberByCaseIdDm(command.CaseId),
                        cancellationToken)
                    .Map(dm => (x.username, x.UserId, dm.RadNumber)))

            // 4️⃣ Prepare Payload (Factory)
            .Bind(x =>
                requestFacilityFactory.PreparePayloadAsync(
                    x.username,
                    x.UserId,
                    x.RadNumber))

            // 5️⃣ Abort Case (External)
            .Bind(workflowEngineService.AbortCaseAsync)

            // 6️⃣ Final DTO
            .Map(_ =>
                new RemoveRequestFromCartableDto(
                    true,
                    DateTime.Now,
                    "درخواست با موفقیت از کارتابل حذف شد."));
    }
}
