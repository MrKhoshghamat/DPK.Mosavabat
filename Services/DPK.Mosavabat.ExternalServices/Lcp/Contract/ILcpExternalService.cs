using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.ExternalServices.Lcp.Contract;

public interface ILcpExternalService
    : ITransientDependency
{
    Task<Result<AcceptAgentDto>> AcceptAgent(
        AcceptAgentDm dm,
        CancellationToken  cancellationToken = default);

    Task<Result<CreateLcpContractDto>> CreateContract(
        CreateLcpContractDm dm,
        CancellationToken cancellationToken);

    Task<Result<List<BoardMemberDto>>> GetBoardMembers(
        GetBoardMembersDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<CreateTicketDto>> CreateTicket(
        CreateTicketDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<UpdateLcpRequestFacilityStatusDto>> UpdateLcpRequestFacilityStatus(
        UpdateLcpRequestFacilityStatusDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<UpdateLcpRequestFacilityRadNumberDto>> UpdateLcpRequestFacilityRadNumber(
        UpdateLcpRequestFacilityRadNumberDm dm,
        CancellationToken cancellationToken = default);
}