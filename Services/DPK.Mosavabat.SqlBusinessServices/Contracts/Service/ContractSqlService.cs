using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.DataModels.Contracts;
using DPK.Mosavabat.Models.EntitiesDto.Contracts;
using DPK.Mosavabat.SqlBusinessServices.Contracts.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Dapper;
using System.Data;
using DPK.Mosavabat.Common.Primitives;
using System.Linq;
using ContractDto = DPK.Mosavabat.Models.EntitiesDto.Contracts.ContractDto;

namespace DPK.Mosavabat.SqlBusinessServices.Contracts.Service;

public class ContractSqlService(
    IConfiguration configuration,
    ILogger<ContractSqlService> logger)
    : IContractSqlService
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

    public async Task<Result<UpdateContractFileLinkDto>> UpdateContractFileLink(
        UpdateContractFileLinkDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[UpdateContractFileLink]";
        var parameters = new
        {
            dm.FileLink,
            dm.CuApprovalSubjectId
        };

        var rowsAffected = await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            return await connection.ExecuteAsync(command);
        }, cancellationToken);

        return new UpdateContractFileLinkDto(rowsAffected > 0);
    }

    public async Task<Result<List<ContractDto>>> GetApprovalContracts(
        GetApprovalContractsDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[CP].[GetApprovalContracts]";

        var parameters = new
        {
            dm.MosavabeNo
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
                    .QueryAsync<ContractDto>(
                        command);

                return result.ToList();
            }, cancellationToken);
    }

    public async Task<Result<GetContractTypeTitleByContractTypeIdDto>> GetContractTypeTitleByContractTypeId(
        GetContractTypeTitleByContractTypeIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetContractTypeTitleByContractTypeId]";
        var parameters = new
        {
            contractTypeId = dm.ContractTypeId
        };

        var result = await ExecuteWithLoggingAsync(
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

                return await connection.ExecuteScalarAsync<GetContractTypeTitleByContractTypeIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetContractTypeTitleByContractTypeIdDto>(
                new Error("Sql.ContractTypeTitleNotFound", $"عنوان نوع عقدی با شناسه {dm.ContractTypeId} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetSubContractTypeTitleBySubContractTypeIdDto>> GetSubContractTypeTitleBySubContractTypeId(
        GetSubContractTypeTitleBySubContractTypeIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetContractTypeTitleByContractTypeId]";
        var parameters = new
        {
            subContractTypeId = dm.SubContractTypeId
        };

        var result = await ExecuteWithLoggingAsync(
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

                return await connection.ExecuteScalarAsync<GetSubContractTypeTitleBySubContractTypeIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetSubContractTypeTitleBySubContractTypeIdDto>(
                new Error("Sql.SubContractTypeTitleNotFound", $"عنوان زیر نوع عقدی با شناسه {dm.SubContractTypeId} یافت نشد."));
        }

        return result;
    }
}