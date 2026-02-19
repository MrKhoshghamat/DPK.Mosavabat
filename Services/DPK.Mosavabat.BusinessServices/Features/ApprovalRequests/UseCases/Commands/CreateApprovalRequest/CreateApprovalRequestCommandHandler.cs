using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Rules;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.Models.Commands.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.UseCases.Commands.CreateApprovalRequest;

/// <summary>
/// هندلر سرویس ایجاد درخواست صدور مصوبه
/// </summary>
internal sealed class CreateApprovalRequestCommandHandler(
    IApprovalRequestSqlService approvalRequestSqlService,
    IWorkflowEngineService workflowEngineService,
    IRequestFacilitySqlService requestFacilitySqlService,
    ILcpExternalService lcpExternalService)
    : ICommandHandler<
        CreateApprovalRequestCommand,
        Result<CreateApprovalRequestDto>>
{
    private const string ProcessName = "MainProcess";
    private const string EntityName = "approvalsRequest";

    public async Task<Result<CreateApprovalRequestDto>> Handle(
        CreateApprovalRequestCommand command,
        CancellationToken cancellationToken)
    {
        return await approvalRequestSqlService
            // 1️⃣ Load BranchCode DM
            .GetBranchCodeByLcpRequestFacilityId(
                new GetBranchCodeByLcpRequestFacilityIdDm(
                    command.LcpRequestFacilityId),
                cancellationToken)

            // ✅ Extract primitive
            .Map(dm => dm.BranchCode)

            // 2️⃣ Ensure BranchCode
            .Bind(ApprovalRequestRules.ValidBranchCode)

            // 3️⃣ Load Username
            .Bind(branchCode =>
                approvalRequestSqlService.GetUsernameByBranchCode(
                    new GetUsernameByBranchCodeDm(
                        branchCode,
                        MosavabatClassName.ApprovalRequest2),
                    cancellationToken))

            // ✅ Extract username
            .Map(dm => dm.Username)

            // 4️⃣ Ensure Username
            .Bind(ApprovalRequestRules.UsernameExists)

            // 5️⃣ Create Bizagi Case
            .Bind(username =>
                workflowEngineService.CreateCaseAsync(
                    new CreateApprovalRequestBizagiDm
                    {
                        ApprovalRequestGroup = "1",
                        RequestTypeDetail = "1"
                    },
                    processName: ProcessName,
                    entityName: EntityName,
                    userName: username))

            // 6️⃣ Ensure Bizagi Result
            .Bind(ApprovalRequestRules.ValidBizagiCase)

            // 7️⃣ Side‑Effect: Update RAD Number (External)
            .Tap(caseResult =>
                lcpExternalService.UpdateLcpRequestFacilityRadNumber(
                    new UpdateLcpRequestFacilityRadNumberDm(
                        command.LcpRequestFacilityIdCase,
                        Convert.ToInt32(caseResult.CaseId),
                        Convert.ToInt32(caseResult.CaseId)),
                    cancellationToken))

            // 8️⃣ Side‑Effect: Update Request Facility (SQL)
            .Tap(caseResult =>
                requestFacilitySqlService.UpdateLcpRequestFacility(
                    new UpdateLcpRequestFacilityDm(
                        command.LcpRequestFacilityIdCase,
                        command.LcpRequestFacilityRadNumber,
                        Convert.ToInt32(caseResult.CaseId),
                        Convert.ToInt32(caseResult.RadNumber),
                        command.LcpRequestFacilityId,
                        command.GuidLcpRequestFacility),
                    cancellationToken))

            // 9️⃣ Side‑Effect: Mapping RF → ApprovalRequest
            .Tap(caseResult =>
                requestFacilitySqlService.MapLcpRequestFacilityToApprovalRequest(
                    new MapLcpRequestFacilityToApprovalRequestDm(
                        command.LcpRequestFacilityId,
                        Convert.ToInt32(caseResult.CaseId),
                        command.LcpRequestFacilityIdCase),
                    cancellationToken))

            // 🔟 Final DTO
            .Map(caseResult =>
                new CreateApprovalRequestDto(
                    caseResult.CaseId,
                    caseResult.RadNumber,
                    DateTime.Now,
                    "فرایند با موفقیت ایجاد شد"));
    }
}