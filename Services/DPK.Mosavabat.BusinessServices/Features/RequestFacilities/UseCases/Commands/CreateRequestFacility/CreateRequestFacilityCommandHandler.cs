using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.UseCases.Commands.CreateRequestFacility;

/// <summary>
/// هندلر سرویس ایجاد درخواست تسهیلات
/// </summary>
internal sealed class CreateRequestFacilityCommandHandler(
    IRequestFacilitySqlService requestFacilitySqlService,
    IWorkflowEngineService workflowEngineService,
    IMapper mapper)
    : ICommandHandler<
        CreateRequestFacilityCommand,
        Result<CreateRequestFacilityDto>>
{
    public record PipelineContext(
        CreateRequestFacilityCommand Command,
        CreateRequestFacilityBizagiDm BizagiDm,
        string Guid,
        string Username = "",
        int BizagiCaseId = 0,
        string BizagiRadNumber = "");

    public async Task<Result<CreateRequestFacilityDto>> Handle(
        CreateRequestFacilityCommand command,
        CancellationToken cancellationToken)
    {
        var initialBizagiDm = mapper.Map<CreateRequestFacilityBizagiDm>(command);
        var initialContext = new PipelineContext(
            command,
            initialBizagiDm,
            Guid.NewGuid().ToString());

        return await Result.Success(initialContext)
            .Bind(context => FetchUsernameAsync(context, cancellationToken))
            .Bind(context => EnrichCollateralsAsync(context, cancellationToken))
            .Bind(context => EnrichSubjectFacilitiesAsync(context, cancellationToken))
            .Bind(CallWorkflowEngineAsync)
            .Bind(context => UpdateLcpRequestFacilityInDbAsync(context, cancellationToken))
            .Map(CreateFinalResponse);
    }

    // =================================================================================
    // PIPELINE STEPS
    // =================================================================================

    private async Task<Result<PipelineContext>> FetchUsernameAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var usernameResult = await requestFacilitySqlService.GetUsernameByBranchCode(
            new GetUsernameByBranchCodeDm(
                int.Parse(context.Command.BranchCode),
                MosavabatClassName.LcpRequestFacility),
            cancellationToken);

        return usernameResult.IsFailure
            ? Result.Failure<PipelineContext>(usernameResult.Error)
            : Result.Success(context with { Username = usernameResult.Value.Username });
    }

    private async Task<Result<PipelineContext>> EnrichCollateralsAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var collateralTasks = context.Command.Collaterals.Collaterals.Select(async collateral =>
        {
            var typeIdResult = await requestFacilitySqlService.GetCollateralTypeId(
                new GetCollateralTypeIdDm(int.Parse(collateral.CollateralType!)),
                cancellationToken);

            return new { Collateral = collateral, Result = typeIdResult };
        });

        var completedTasks = await Task.WhenAll(collateralTasks);

        var firstFailure = completedTasks.FirstOrDefault(t => t.Result.IsFailure);
        if (firstFailure != null)
            return Result.Failure<PipelineContext>(firstFailure.Result.Error);

        foreach (var item in completedTasks)
        {
            context.BizagiDm.LcpCollaterals.Add(new CollateralBizagiDm()
            {
                Amount = item.Collateral.Amount!.ToThousandSeparatedWithOutDecimals(),
                CollateralType = item.Result.Value.CollateralTypeId
            });
        }

        return Result.Success(context);
    }

    private async Task<Result<PipelineContext>> EnrichSubjectFacilitiesAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var commandFacilities = context.Command.SubjectFacilities.SubjectFacilities.ToList();
        var bizagiFacilities = context.BizagiDm.SubjectFacilities.ToList();

        for (var i = 0; i < commandFacilities.Count; i++)
        {
            var sourceSubject = commandFacilities[i];
            var targetBizagiSubject = bizagiFacilities[i];

            var documentTasks = sourceSubject.RequestDocuments.RequestDocuments.Select(async doc =>
            {
                var docIdResult = await requestFacilitySqlService.ExtractExchangeCommercialDocId(
                    new ExtractExchangeCommercialDocIdDm(doc.DocumentType),
                    cancellationToken);
                return new { Document = doc, Result = docIdResult };
            });

            var completedDocs = await Task.WhenAll(documentTasks);

            var firstDocFailure = completedDocs.FirstOrDefault(d => d.Result.IsFailure);
            if (firstDocFailure != null)
                return Result.Failure<PipelineContext>(firstDocFailure.Result.Error);

            foreach (var item in completedDocs)
            {
                targetBizagiSubject.RequestDocuments.Add(new ContractDocumentBizagiDm()
                {
                    Amount = item.Document.Amount.ToThousandSeparatedWithOutDecimals(),
                    DocumentNumber = item.Document.DocumentNumber,
                    DocumentType = item.Result.Value.ExchangeCommercialDocId,
                    DueDate = item.Document.DueDate,
                    LcpId = item.Document.LcpId,
                    Owner = item.Document.Owner,
                    PayAmount = item.Document.PayAmount
                });
            }

            targetBizagiSubject.Description = sourceSubject.Description?.Replace("&amp;nbsp;", "") ?? string.Empty;
            targetBizagiSubject.SubContractType = sourceSubject.SubContractType?.Replace("&amp;nbsp;", "") ?? string.Empty;

            var subContractTask = requestFacilitySqlService.ExtractSubContractTypeId(
                new ExtractSubContractTypeIdDm(int.Parse(sourceSubject.SubContractType!)),
                cancellationToken);

            var facilityGroupTask = requestFacilitySqlService.ExtractFacilityGroupId(
                new ExtractFacilityGroupIdDm(sourceSubject.ContractType),
                cancellationToken);

            await Task.WhenAll(subContractTask, facilityGroupTask);

            if (subContractTask.Result.IsFailure)
                return Result.Failure<PipelineContext>(subContractTask.Result.Error);
            if (facilityGroupTask.Result.IsFailure)
                return Result.Failure<PipelineContext>(facilityGroupTask.Result.Error);

            targetBizagiSubject.SubContractType = subContractTask.Result.Value.ContractTemplateId;
            targetBizagiSubject.FacilityGroup = facilityGroupTask.Result.Value.FacilityGroupId;
        }

        return Result.Success(context);
    }

    private async Task<Result<PipelineContext>> CallWorkflowEngineAsync(
        PipelineContext context)
    {
        var caseResult = await workflowEngineService.CreateCaseAsync(
            context.BizagiDm,
            "MainProcess",
            "LCP_RequestFacility",
            context.Username);

        if (caseResult.IsFailure)
            return Result.Failure<PipelineContext>(caseResult.Error);

        return Result.Success(context with
        {
            BizagiCaseId = Convert.ToInt32(caseResult.Value.CaseId),
            BizagiRadNumber = caseResult.Value.RadNumber
        });
    }

    private async Task<Result<PipelineContext>> UpdateLcpRequestFacilityInDbAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        var dm = new UpdateLcpRequestFacilityDm(
            context.BizagiCaseId,
            int.Parse(context.BizagiRadNumber),
            0,
            0,
            0,
            context.Guid);

        var updateResult = await requestFacilitySqlService
            .UpdateLcpRequestFacility(
                dm,
                cancellationToken);

        return updateResult.IsFailure
            ? Result.Failure<PipelineContext>(updateResult.Error)
            : Result.Success(context);
    }

    private static CreateRequestFacilityDto CreateFinalResponse(PipelineContext context)
    {
        return new CreateRequestFacilityDto(
            context.BizagiCaseId,
            context.BizagiRadNumber,
            DateTime.Now,
            "عملیات موفقیت آمیز بود");
    }
}
