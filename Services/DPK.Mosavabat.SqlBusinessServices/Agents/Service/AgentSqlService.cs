using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.Agents;
using DPK.Mosavabat.Models.EntitiesDto.Agents;
using DPK.Mosavabat.SqlBusinessServices.Agents.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using GetRadNumberByCaseIdDm = DPK.Mosavabat.Models.DataModels.Agents.GetRadNumberByCaseIdDm;
using GetRadNumberByCaseIdDto = DPK.Mosavabat.Models.EntitiesDto.Agents.GetRadNumberByCaseIdDto;
using GetUserIdByUsernameDm = DPK.Mosavabat.Models.DataModels.Agents.GetUserIdByUsernameDm;
using GetUserIdByUsernameDto = DPK.Mosavabat.Models.EntitiesDto.Agents.GetUserIdByUsernameDto;
using GetUserNameByCaseIdDm = DPK.Mosavabat.Models.DataModels.Agents.GetUserNameByCaseIdDm;
using GetUserNameByCaseIdDto = DPK.Mosavabat.Models.EntitiesDto.Agents.GetUserNameByCaseIdDto;

namespace DPK.Mosavabat.SqlBusinessServices.Agents.Service;

public class AgentSqlService(
    IConfiguration configuration,
    ILogger<AgentSqlService> logger) : IAgentSqlService
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

    public async Task<Result<UpdateCaseIdAndRadNumberInAgentRegistryDto>> UpdateCaseIdAndRadNumberInAgentRegistry(
        UpdateCaseIdAndRadNumberInAgentRegistryDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[UpdateCaseIdAndRadNumberInAgentRegistry]";
        var parameters = new
        {
            branchCode = dm.AgentDm.BranchCode,
            nationalCode = dm.AgentDm.NationalCode,
            lastName = dm.AgentDm.LastName,
            mobile = dm.AgentDm.Mobile,
            name = dm.AgentDm.Name,
            legalNationalId = dm.AgentDm.LegalNationalId,
            legalName = dm.AgentDm.LegalName,
            caseId = dm.CaseId,
            radNumber = dm.RadNumber
        };

        var rowsAffected = await ExecuteWithLoggingAsync(
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

                return await connection.ExecuteAsync(command);
            }, cancellationToken);

        return new UpdateCaseIdAndRadNumberInAgentRegistryDto(rowsAffected > 0);
    }

    public async Task<Result<GetUserNameByCaseIdDto>> GetUserNameByCaseId(
        GetUserNameByCaseIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetUserNameByCaseId]";
        var parameters = new { caseId = dm.CaseId };

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

                return await connection.ExecuteScalarAsync<GetUserNameByCaseIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetUserNameByCaseIdDto>(
                new Error("Sql.UserNotFound", $"کاربری برای کیس {dm.CaseId} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetUserIdByUsernameDto>> GetUserIdByUsername(
        GetUserIdByUsernameDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetUserIdByUsername]";
        var parameters = new { username = dm.Username };

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

                return await connection.ExecuteScalarAsync<GetUserIdByUsernameDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetUserIdByUsernameDto>(
                new Error("Sql.UserNotFound", $"کاربری با نام کاربری {dm.Username} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetRadNumberByCaseIdDto>> GetRadNumberByCaseId(
        GetRadNumberByCaseIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetRadNumberByCaseId]";
        var parameters = new { caseId = dm.CaseId };

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

                return await connection.ExecuteScalarAsync<GetRadNumberByCaseIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetRadNumberByCaseIdDto>(
                new Error("Sql.RadNumberNotFound", $"شماره پرونده (RadNumber) برای کیس {dm.CaseId} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<AbortAgentCaseDto>> AbortAgentCase(
        AbortAgentCaseDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[LCP_AgentAbortCase]";
        var parameters = new
        {
            caseId = dm.CaseId
        };

        var rowsAffected = await ExecuteWithLoggingAsync(
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

                return await connection.ExecuteAsync(command);
            }, cancellationToken);

        return new AbortAgentCaseDto(rowsAffected > 0);
    }
}
