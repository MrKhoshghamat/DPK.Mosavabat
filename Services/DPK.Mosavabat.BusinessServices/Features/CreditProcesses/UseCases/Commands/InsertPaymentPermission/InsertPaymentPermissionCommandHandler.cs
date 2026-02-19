using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.BusinessServices.Abstractions.Errors;
using DPK.Mosavabat.Common.Exceptions;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.ExternalServices.Payam.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.DataModels.Payam;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;
using BusinessRuleException = DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.ControlPaymentPermissionIssuance.BusinessRuleException;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.InsertPaymentPermission;

/// <summary>
/// ایجاد شناسه پرداخت
/// </summary>
internal class InsertPaymentPermissionCommandHandler(
    ICreditProcessSqlService creditProcessSqlService,
    IPayamExternalService payamExternalService,
    ICreditProcessMessageProvider messageProvider,
    ILmsExternalService lmsExternalService,
    IMapper mapper)
    : ICommandHandler<InsertPaymentPermissionCommand,
        InsertPaymentPermissionDto>
{
    public async Task<InsertPaymentPermissionDto> Handle(
        InsertPaymentPermissionCommand command,
        CancellationToken cancellationToken)
    {
        var paymentPermissionIssuanceControlResponse = await creditProcessSqlService
            .ControlPaymentPermissionIssuance(
                new ControlPaymentPermissionIssuanceDm(
                    command.MosavabeNo,
                    command.ContractAmount,
                    command.PrepaidAmount,
                    command.SubContractType,
                    command.CustomerNo),
                cancellationToken);

        ValidateDatabaseResult(
            paymentPermissionIssuanceControlResponse.Result);

        var checkInquiryResponse = await payamExternalService.CheckInquiry(
            new CheckInquiryDm(
                paymentPermissionIssuanceControlResponse.Result),
            cancellationToken);

        ValidateDatabaseResult(
            checkInquiryResponse
                .BouncedCheckCount);

        var insertPaymentPermissionResponse = await lmsExternalService.InsertPaymentPermission(
            mapper.Map<InsertPaymentPermissionDm>(command),
            cancellationToken);

        return insertPaymentPermissionResponse;
    }

    private void ValidateDatabaseResult(string? result)
    {
        if (string.IsNullOrEmpty(result) ||
            result.Length > 5 ||
            result.Equals("true", StringComparison.OrdinalIgnoreCase) ||
            result.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (!int.TryParse(
                result,
                out var code)) return;

        if (messageProvider
            .TryGetMessage(
                code,
                out var message,
                out var errorEnum))
        {
            throw new BusinessRuleException(
                errorEnum!.Value,
                message);
        }
    }
}