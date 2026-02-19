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

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.UpdateRequestFacility;

/// <summary>
/// هندلر سرویس ویرایش درخواست تسهیلات
/// </summary>
internal class UpdateRequestFacilityCommandHandler(
    IRequestFacilitySqlService requestFacilitySqlService,
    IWorkflowEngineService workflowEngineService,
    IRequestFacilityFactory requestFacilityFactory)
    : ICommandHandler<
        UpdateRequestFacilityCommand,
        Result<UpdateRequestFacilityDto>>
{
    public Task<Result<UpdateRequestFacilityDto>> Handle(
        UpdateRequestFacilityCommand command,
        CancellationToken cancellationToken)
    {
        return requestFacilitySqlService
            // 1️⃣ Load ApprovalRequestId
            .GetApprovalRequestId(
                new GetApprovalRequestIdDm(int.Parse(command.CaseId)),
                cancellationToken)

            .Map(dm => dm.ApprovalRequestId)

            // 2️⃣ Prepare Payload
            .Bind(approvalRequestId =>
                requestFacilityFactory.PreparePayloadAsync(
                    command,
                    approvalRequestId.ToString()))

            // 3️⃣ Update Status (Side‑Effect with Result)
            .Tap(_ =>
                requestFacilitySqlService.UpdateRequestFacilityStatus(
                    new UpdateRequestFacilityStatusDm(
                        int.Parse(command.CaseId)),
                    cancellationToken))

            // 4️⃣ Save Entity in Bizagi
            .Bind(workflowEngineService.SaveEntityAsync)

            // 5️⃣ Final DTO
            .Map(result =>
                new UpdateRequestFacilityDto(
                    Convert.ToInt64(result.CaseId),
                    "Updated",
                    DateTime.Now,
                    "ویرایش با موفقیت در بیزاجی اعمال شد."));
    }
}
