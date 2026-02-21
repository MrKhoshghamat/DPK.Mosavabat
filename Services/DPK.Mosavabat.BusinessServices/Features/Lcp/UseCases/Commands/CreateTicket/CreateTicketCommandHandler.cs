using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.Models.Commands.Lcp;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.SqlBusinessServices.Lcp.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.UseCases.Commands.CreateTicket;

/// <summary>
/// هندلر سرویس ثبت درخواست تیکت
/// </summary>
internal class CreateTicketCommandHandler(
    ILcpExternalService lcpExternalService,
    ILcpSqlService lcpSqlService,
    IMapper mapper)
    : ICommandHandler<
        CreateTicketCommand,
        Result<CreateTicketDto>>
{
    private record PipelineContext(
        CreateTicketCommand Command,
        CreateTicketDto? CreatedTicketDto = null);

    public async Task<Result<CreateTicketDto>> Handle(
        CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var initialContext = new PipelineContext(command);

        return await Result.Success(initialContext)
            .Bind(context => CreateTicketInExternalServiceAsync(context, cancellationToken))
            .Bind(context => UpdateLocalStatusInDatabaseAsync(context, cancellationToken))
            .Map(context => context.CreatedTicketDto!);
    }

    private async Task<Result<PipelineContext>> CreateTicketInExternalServiceAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            var ticketDto = await lcpExternalService
                .CreateTicket(
                    mapper.Map<CreateTicketDm>(
                        context.Command),
                    cancellationToken);

            return Result.Success(context with { CreatedTicketDto = ticketDto.Value });
        }
        catch (Exception)
        {
            return Result.Failure<PipelineContext>(
                new Error(
                    "CreateTicket.ExternalServiceError",
                    "خطا در ارتباط با سرویس خارجی جهت ثبت تیکت."));
        }
    }

    private async Task<Result<PipelineContext>> UpdateLocalStatusInDatabaseAsync(
        PipelineContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            var statusDm = new InsertLcpRequestFacilityStatusDm(context.Command.RequestFacilityId);

            await lcpSqlService.InsertRequestFacilityStatus(statusDm, cancellationToken);

            return Result.Success(context);
        }
        catch (Exception)
        {
            return Result.Failure<PipelineContext>(
                new Error(
                    "CreateTicket.LocalDatabaseError",
                    "تیکت ثبت شد اما بروزرسانی وضعیت سیستم با خطا مواجه شد."));
        }
    }
}
