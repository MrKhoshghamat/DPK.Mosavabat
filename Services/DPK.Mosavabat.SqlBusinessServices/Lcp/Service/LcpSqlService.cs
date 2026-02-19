using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;
using Dapper;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.SqlBusinessServices.Lcp.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DPK.Mosavabat.SqlBusinessServices.Lcp.Service;

public class LcpSqlService(
    IConfiguration configuration,
    ILogger<LcpSqlService> logger)
    : ILcpSqlService
{
    private readonly string? _connectionString = configuration.GetConnectionString("MosavabatConnection");

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

    public async Task<InsertLcpRequestFacilityStatusDto> InsertRequestFacilityStatus(
        InsertLcpRequestFacilityStatusDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[LCP].[InsertReqFacilityStatus]";
        var parameters = new
        {
            idRequestFacility = dm.IdRequestFacility,
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

        return new InsertLcpRequestFacilityStatusDto(rowsAffected > 0);
    }
}
