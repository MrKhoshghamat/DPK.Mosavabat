using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.UI;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Bizagi.WorkFlowEngine;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;
using Microsoft.Extensions.Logging;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CreateCompleteContractInfoProcess;

/// <summary>
/// هندلر سرویس ساخت فرایند تکمیل اطلاعات قرارداد
/// </summary>
internal class CreateCompleteContractInfoProcessCommandHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IWorkflowEngineService workflowEngineService,
    ILogger<CreateCompleteContractInfoProcessCommandHandler> logger)
    : ICommandHandler<
        CreateCompleteContractInfoProcessCommand,
        Result<CreateCompleteContractInfoProcessDto>>
{
    private const string ProcessName = "MainProcess";
    private const string EntityName = "CU_Contract";

    public async Task<Result<CreateCompleteContractInfoProcessDto>> Handle(
        CreateCompleteContractInfoProcessCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("شروع درخواست ایجاد فرایند تکمیل قرارداد برای C13: {C13}", command.C13);

        var branchDto = await creditProcessSqlService.GetBranchCodeByC13(
            new GetBranchCodeByC13Dm(command.C13), cancellationToken);

        if (!int.TryParse(branchDto.BranchCode, out var branchCodeInt))
            throw new UserFriendlyException("کد شعبه نامعتبر است.");

        var userDto = await creditProcessSqlService.FindBranchManagerOrSupervisor(
            new FindBranchManagerOrSupervisorDm(branchCodeInt, MosavabatClassName.CompleteContractInfo),
            cancellationToken);

        if (string.IsNullOrEmpty(userDto.Username))
        {
            logger.LogWarning("کاربر مسئول برای ایجاد فرایند یافت نشد. C13: {C13}", command.C13);
            return new CreateCompleteContractInfoProcessDto(
                0,
                "0",
                DateTime.Now,
                "کاربر مسئول یافت نشد.");
        }

        var accNoDto = await creditProcessSqlService.GetAccNoByC13(
            new GetAccNoByC13Dm(
                command.C13),
            cancellationToken);

        var dateGDto = await creditProcessSqlService.GetDateGByC13(
            new GetDateGByC13Dm(
                command.C13),
            cancellationToken);

        var c16Dto = await creditProcessSqlService.GetC16ByC13(
            new GetC16ByC13Dm(
                command.C13),
            cancellationToken);

        var bizagiPayload = new CompleteContractInfoBizagiDm
        {
            MabAsl = command.MabAsl,
            CuApprovalSubjectId = command.CuApprovalSubjectId,
            C10 = command.C10,
            Shob = branchDto.BranchCode,
            C13 = command.C13,
            C16 = c16Dto.C16,
            DateG = dateGDto.DateG
        };

        logger.LogInformation(
            "در حال ارسال درخواست به بیزاجی برای کاربر: {User}",
            userDto.Username);

        var caseResult = await workflowEngineService.CreateCaseAsync(
            model: bizagiPayload,
            processName: ProcessName,
            entityName: EntityName,
            userName: userDto.Username
        );

        if (caseResult.CaseId == 0 || caseResult.RadNumber == "0")
        {
            logger.LogError("خطا در ایجاد کیس بیزاجی. نتیجه نامعتبر.");
            return new CreateCompleteContractInfoProcessDto(
                0,
                "0",
                DateTime.Now,
                "خطا در ایجاد فرایند در سامانه مصوبات.");
        }

        try
        {
            var customerEkDto = await creditProcessSqlService
                .GetCustomerEk(
                    new GetCustomerEkDm(
                        accNoDto.AccNo),
                    cancellationToken);

            await creditProcessSqlService.UpdateCuCustomerInCuContract(
                new UpdateCuCustomerInCuContractDm(
                    customerEkDto.IdCuCustomerEk,
                    command.C13,
                    c16Dto.C16),
                cancellationToken);

            logger.LogInformation("اطلاعات قرارداد در دیتابیس داخلی بروزرسانی شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "کیس {CaseId} ایجاد شد اما بروزرسانی دیتابیس داخلی با خطا مواجه شد.", caseResult.CaseId);
        }

        return new CreateCompleteContractInfoProcessDto(
            caseResult.CaseId,
            caseResult.RadNumber,
            DateTime.Now,
            "فرایند با موفقیت ایجاد شد.");
    }
}