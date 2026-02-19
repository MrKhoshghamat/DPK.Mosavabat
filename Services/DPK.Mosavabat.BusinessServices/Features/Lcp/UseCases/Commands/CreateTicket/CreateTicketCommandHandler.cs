using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
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
        CreateTicketDto>
{
    public async Task<CreateTicketDto> Handle(
        CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var createTicketDto = await lcpExternalService.CreateTicket(
            mapper.Map<CreateTicketDm>(command),
            cancellationToken);

        await lcpSqlService.InsertRequestFacilityStatus(
            new InsertLcpRequestFacilityStatusDm(
                command.RequestFacilityId),
            cancellationToken);

        return createTicketDto;
    }
}