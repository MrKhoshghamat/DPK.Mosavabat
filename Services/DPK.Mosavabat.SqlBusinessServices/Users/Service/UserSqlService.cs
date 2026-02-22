using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.Users;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.Users;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Service;
using DPK.Mosavabat.SqlBusinessServices.Users.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DPK.Mosavabat.SqlBusinessServices.Users.Service;

public class UserSqlService(
    IConfiguration configuration,
    ILogger<CreditProcessSqlService> logger)
    : IUserSqlService
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

    public async Task<Result<GetUsernameByBranchCodeDto>> GetUsernameByBranchCode(
        GetUsernameByBranchCodeDm dm,
        CancellationToken cancellationToken = default)
    {
        var commandText = dm.MosavabatClassName == MosavabatClassName.AgentRegistry
            ? "[LCP].[FindBranchManagerOrSupervisor]"
            : "[LCP].[FindFacilityManagerOrSupervisor]";

        var parameters = new { dm.BranchCode };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetUsernameByBranchCodeDto>(command);
            return result!;
        }, cancellationToken);
    }
}