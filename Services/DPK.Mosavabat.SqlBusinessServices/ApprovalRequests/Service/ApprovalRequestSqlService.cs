using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dapper;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DPK.Mosavabat.SqlBusinessServices.ApprovalRequests.Service;

public class ApprovalRequestSqlService(
    IConfiguration configuration,
    ILogger<ApprovalRequestSqlService> logger)
    : IApprovalRequestSqlService
{
    private readonly string? _connectionString = configuration
        .GetConnectionString("MosavabatConnection");

    private async Task<T> ExecuteWithLoggingAsync<T>(
        string commandText,
        object parameters,
        Func<SqlConnection, Task<T>> databaseAction,
        CancellationToken cancellationToken,
        [CallerMemberName] string operationName = "")
    {
        using (LogContext.PushProperty("Layer", "SqlService"))
        using (LogContext.PushProperty("DapperCommand", commandText))
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                var result = await databaseAction(connection);

                sw.Stop();

                if (sw.ElapsedMilliseconds > 500)
                {
                    logger.LogWarning(
                        "Long running SQL {OperationName} took {ElapsedMilliseconds} ms. Parameters: {@Parameters}",
                        operationName, sw.ElapsedMilliseconds, parameters);
                }

                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                logger.LogError(ex,
                    "SQL Error in {OperationName} after {ElapsedMilliseconds} ms. Command: {CommandText}, Parameters: {@Parameters}",
                    operationName, sw.ElapsedMilliseconds, commandText, parameters);

                throw;
            }
        }
    }

    public async Task<Result<GetBranchCodeByLcpRequestFacilityIdDto>> GetBranchCodeByLcpRequestFacilityId(
        GetBranchCodeByLcpRequestFacilityIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[ExtractBranchCode]";
        var parameters = new
        {
            idLcpRequestFacility = dm.IdLcpRequestFacility
        };

        return await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                var result = await connection
                    .ExecuteScalarAsync<GetBranchCodeByLcpRequestFacilityIdDto>(command);

                if (result is null)
                {
                    return Result.Failure<GetBranchCodeByLcpRequestFacilityIdDto>(
                        new Error("Sql.NotFound", $"Branch code not found for FacilityId: {dm.IdLcpRequestFacility}"));
                }

                return Result.Success(result);
            }, cancellationToken);
    }

    public async Task<Result<GetUsernameByBranchCodeDto>> GetUsernameByBranchCode(
        GetUsernameByBranchCodeDm dm,
        CancellationToken cancellationToken = default)
    {
        var commandText = dm.MosavabatClassName == MosavabatClassName.AgentRegistry
            ? "[LCP].[FindBranchManagerOrSupervisor]"
            : "[LCP].[FindFacilityManagerOrSupervisor]";

        var parameters = new
        {
            branchCode = dm.BranchCode
        };

        return await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                var result = await connection
                    .ExecuteScalarAsync<GetUsernameByBranchCodeDto>(command);

                if (result is null)
                {
                    return Result.Failure<GetUsernameByBranchCodeDto>(
                        new Error("Sql.NotFound", $"User not found for BranchCode: {dm.BranchCode}"));
                }

                return Result.Success(result);
            }, cancellationToken);
    }

    public async Task<Result<List<ApprovalRequestsLastStatusDto>>> GetRequestsLastStatuses(
        GetApprovalRequestsLastStatusDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[GetRequestsLastStatus]";

        var xml = new XElement(
            "Ids",
            dm.CaseIds
                .Distinct()
                .Select(id => new XElement("Id", id)));

        var xmlString = xml.ToString();

        var parameters = new
        {
            caseIdsXml = xmlString
        };

        return await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                var result = await connection
                    .QueryAsync<ApprovalRequestsLastStatusDto>(command);

                return Result.Success(result.ToList());
            }, cancellationToken);
    }

    public async Task<Result<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>> IsExistFacilitySubTypeCodeInGroupCreditPackage(
        IsExistFacilitySubTypeCodeInGroupCreditPackageDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[IsExistFacilitySubTypeCodeInGroupCreditPackage]";
        var parameters = new
        {
            dm.FacilitySubTypeCode
        };

        return await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                var result = await connection
                    .ExecuteScalarAsync<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>(command);

                if (result is null)
                {
                    return Result.Failure<IsExistFacilitySubTypeCodeInGroupCreditPackageDto>(
                        new Error("Sql.NotFound", "Facility SubType Code check returned no result."));
                }

                return Result.Success(result);
            }, cancellationToken);
    }

    public async Task<Result<List<ApprovalRequestStatusDto>>> GetApprovalRequestStatuses(
        GetApprovalRequestStatusesDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[GetRequestStatus]";

        var parameters = new
        {
            idCase = dm.IdCase
        };

        return await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                var result = await connection
                    .QueryAsync<ApprovalRequestStatusDto>(command);

                return Result.Success(result.ToList());
            }, cancellationToken);
    }
}
