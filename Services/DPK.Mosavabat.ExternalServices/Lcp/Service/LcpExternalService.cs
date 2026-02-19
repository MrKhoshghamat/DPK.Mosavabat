using AutoMapper;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.ExternalServices.Lcp.Proxy;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.DataModels.Proxies.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using Refit;

namespace DPK.Mosavabat.ExternalServices.Lcp.Service;

public class LcpExternalService(
    ILcpProxyService lcpProxyService,
    IMapper mapper)
    : ILcpExternalService
{
    private const string ServiceName = "LcpExternalService";

    private async Task<Result<TResult>> ExecuteSafeAsync<TResult, TResponse>(
        Func<Task<TResponse>> apiCall)
        where TResponse : IApiResponse<TResult>
    {
        try
        {
            // اجرای درخواست
            var response = await apiCall();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content is not null)
                {
                    return Result.Success(response.Content);
                }

                return Result.Failure<TResult>(
                    new Error($"{ServiceName}.NullContent", $"Service returned {response.StatusCode} but content was null."));
            }

            var errorDetail = response.Error?.Content
                              ?? response.ReasonPhrase
                              ?? "Unknown External Error";

            return Result.Failure<TResult>(
                new Error($"{ServiceName}.Failure", $"Status: {response.StatusCode} | Detail: {errorDetail}"));
        }
        catch (Exception ex)
        {
            return Result.Failure<TResult>(
                new Error($"{ServiceName}.Exception", ex.Message));
        }
    }

    public async Task<Result<AcceptAgentDto>> AcceptAgent(
        AcceptAgentDm dm,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteSafeAsync<AcceptAgentDto, ApiResponse<AcceptAgentDto>>(
            () => lcpProxyService.AcceptAgentAsync(
                mapper.Map<AcceptAgentProxyDm>(dm),
                cancellationToken));
    }

    public async Task<Result<CreateLcpContractDto>> CreateContract(
        CreateLcpContractDm dm,
        CancellationToken cancellationToken)
    {
        return await ExecuteSafeAsync<CreateLcpContractDto, ApiResponse<CreateLcpContractDto>>(
            () => lcpProxyService.CreateContractAsync(
                mapper.Map<CreateLcpContractProxyDm>(dm),
                cancellationToken));
    }

    public async Task<Result<List<BoardMemberDto>>> GetBoardMembers(
        GetBoardMembersDm dm,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteSafeAsync<List<BoardMemberDto>, ApiResponse<List<BoardMemberDto>>>(
            () => lcpProxyService.GetBoardMembersAsync(
                mapper.Map<GetBoardMembersProxyDm>(dm),
                cancellationToken));
    }

    public async Task<Result<CreateTicketDto>> CreateTicket(
        CreateTicketDm dm,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteSafeAsync<CreateTicketDto, ApiResponse<CreateTicketDto>>(
            () => lcpProxyService.CreateTicketAsync(
                mapper.Map<CreateTicketProxyDm>(dm),
                cancellationToken));
    }

    public async Task<Result<UpdateLcpRequestFacilityStatusDto>> UpdateLcpRequestFacilityStatus(
        UpdateLcpRequestFacilityStatusDm dm,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteSafeAsync<UpdateLcpRequestFacilityStatusDto, ApiResponse<UpdateLcpRequestFacilityStatusDto>>(
            () => lcpProxyService.UpdateLcpRequestFacilityStatusAsync(
                mapper.Map<UpdateLcpRequestFacilityStatusProxyDm>(dm),
                cancellationToken));
    }

    public async Task<Result<UpdateLcpRequestFacilityRadNumberDto>> UpdateLcpRequestFacilityRadNumber(
        UpdateLcpRequestFacilityRadNumberDm dm,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteSafeAsync<UpdateLcpRequestFacilityRadNumberDto, ApiResponse<UpdateLcpRequestFacilityRadNumberDto>>(
            () => lcpProxyService.UpdateLcpRequestFacilityRadNumberAsync(
                mapper.Map<UpdateLcpRequestFacilityRadNumberProxyDm>(dm),
                cancellationToken));
    }
}
